using System;
using Emgu.CV;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;
using System.IO;

namespace DetectorFacial
{
    public partial class Form1 : Form
    {
        // Variáveis globais.

        Capture camera;

        HaarCascade Modelo;

        Image <Bgr,  Byte> Frame;
        Image <Gray, byte> result;
        Image <Gray, byte> RostoDetectado;

        public Form1()
        {
            InitializeComponent();

            // "HaarCascade" é para detecção de faces.
            Modelo = new HaarCascade("haarcascade_frontalface_default.xml");
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // Inicia a camera e chama
            camera = new Capture();
            camera.QueryFrame();
            Application.Idle += new EventHandler(processandoCamera);
        }

        private void processandoCamera(object sender, EventArgs e)
        {
            // Análisa os frames e desenha o retângulo, caso encontre um rosto.
            Frame = camera.QueryFrame().Resize(325, 300, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            RostoDetectado = Frame.Convert<Gray, Byte>();
            MCvAvgComp[][] FaceAtual = RostoDetectado.DetectHaarCascade(Modelo, 1.1, 5, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

            foreach (MCvAvgComp f in FaceAtual[0])
            {
                result = Frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                Frame.Draw(f.rect, new Bgr(Color.LightGray), 2);             
            }           

            cameraBox.Image  = Frame;        
        }
    }
}
