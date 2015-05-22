using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using D3D11 = SharpDX.Direct3D11;

namespace GeneralProjectDX
{
    public class Program
    {
        private const int WIDTH = 800;
        private const int HEIGHT = 600;

        private static RenderForm _renderForm;

        private static SwapChain _swapChain;
        private static D3D11.Device _d3d11Device;
        private static DeviceContext _d3d11DevCon;
        private static RenderTargetView _renderTargetView;

        private static Color4 _backgroundColor;
        private static int _colorModR = 1;
        private static int _colorModG = 1;
        private static int _colorModB = 1;

        static void Main(string[] args)
        {
            _renderForm = new RenderForm("My First Game!");
            _renderForm.ClientSize = new System.Drawing.Size(WIDTH, HEIGHT);
            _renderForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            InitializeD3D11();
            InitializeScene();

            RenderLoop.Run(_renderForm, RenderCallback);

            DisposeObjects();
        }

        private static void RenderCallback()
        {
            UpdateScene();
            DrawScene();
        }

        private static void InitializeD3D11()
        {
            ModeDescription bufferDescription = new ModeDescription()
            {
                Width = WIDTH,
                Height = HEIGHT,
                RefreshRate = new Rational(60, 1),
                Format = Format.R8G8B8A8_UNorm
            };

            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                ModeDescription = bufferDescription,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = _renderForm.Handle,
                IsWindowed = true
            };

            D3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDescription, out _d3d11Device, out _swapChain);
            _d3d11DevCon = _d3d11Device.ImmediateContext;

            using (Texture2D backBuffer = _swapChain.GetBackBuffer<Texture2D>(0))
            {
                _renderTargetView = new RenderTargetView(_d3d11Device, backBuffer);
            }

            _d3d11DevCon.OutputMerger.SetRenderTargets(_renderTargetView);
        }

        private static void InitializeScene()
        {

        }

        private static void UpdateScene()
        {
            _backgroundColor.Red += _colorModR * 0.005f;
            _backgroundColor.Green += _colorModG * 0.002f;
            _backgroundColor.Blue += _colorModB * 0.001f;
            if (_backgroundColor.Red >= 1f || _backgroundColor.Red <= 0f) _colorModR *= -1;
            if (_backgroundColor.Green >= 1f || _backgroundColor.Green <= 0f) _colorModG *= -1;
            if (_backgroundColor.Blue >= 1f || _backgroundColor.Blue <= 0f) _colorModB *= -1;
        }

        private static void DrawScene()
        {
            _d3d11DevCon.ClearRenderTargetView(_renderTargetView, _backgroundColor);

            _swapChain.Present(1, PresentFlags.None);
        }

        private static void DisposeObjects()
        {
            _swapChain.Dispose();
            _d3d11Device.Dispose();
            _d3d11DevCon.Dispose();
            _renderTargetView.Dispose();
        }
    }
}
