using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Collections;

namespace Inputs.InputImpls
{
    public class TcpCreateProvider : MonoBehaviour, ICreateEventProvider
    {
        private static TcpCreateProvider _instance;
        private TcpListener listener = null;
        private TcpClient client = null;
        private Texture2D _texture;
        public Texture2D texture => _texture;
        private readonly ReactiveProperty<Vector3> _onCreate = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<Vector3> OnCreate => _onCreate;
        [SerializeField] private float sigma = 2.0f;


        private readonly object lockObj = new object();

        private void StartServer(string host, int port)
        {
            IPAddress ip = IPAddress.Parse(host);
            listener = new TcpListener(ip, port);
            listener.Start();
            listener.BeginAcceptTcpClient(OnConnected, null);
        }

        private void OnConnected(IAsyncResult res)
        {
            client = listener.EndAcceptTcpClient(res);
        }

        private void OnMessage()
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var msg = reader.ReadLine();
                try
                {
                    byte[] bytes = Convert.FromBase64String(msg);
                    _texture = new Texture2D(1024, 1024);
                    _texture.LoadImage(bytes);
                    _onCreate.SetValueAndForceNotify(new Vector3(UnityEngine.Random.value * sigma, 0, 0));
                }
                catch (Exception e)
                {
                    CloseConnection();
                }
            }
            CloseConnection();
        }

        private void CloseConnection()
        {
            client.Close();
            client = null;

            listener.BeginAcceptTcpClient(OnConnected, null);
        }
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                _texture = Resources.Load("Textures/fish_texture") as Texture2D;
                StartServer("127.0.0.1", 9090);
            }
        }

        void Update()
        {
            if (client != null)
            {
                OnMessage();
            }
        }
    }
}