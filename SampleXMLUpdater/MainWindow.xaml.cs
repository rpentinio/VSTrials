using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Xml;

namespace SampleXMLUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] selectedFilePaths;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.Title = "Select a file";
            opd.Filter = "XML files (*.xml)|*.xml";
            opd.FilterIndex = 1;
            opd.Multiselect = true;

            if (opd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedFilePaths = opd.FileNames;
            }
            else
            {
                selectedFilePaths = new string[0];
            }
        }

        private void UpdateXML(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                XmlNodeList nodeList;
                XmlNode root = xmlDoc.DocumentElement;

                nodeList = root.SelectNodes("descendant::book");

                foreach (XmlNode oldNode in nodeList)
                {
                    // Create  new element called "disc"
                    XmlElement newNode = xmlDoc.CreateElement("disc");

                    // Copy children from the old node to the new node
                    foreach (XmlNode child in oldNode.ChildNodes)
                    {
                        XmlNode importedChild = xmlDoc.ImportNode(child, true);
                        newNode.AppendChild(importedChild);
                    }

                    XmlNode parentNode = oldNode.ParentNode;
                    parentNode.ReplaceChild(newNode, oldNode);
                }

                string directoryPath = System.IO.Path.GetDirectoryName(filePath);
                string fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(filePath);
                xmlDoc.Save(System.IO.Path.Combine(directoryPath, fileNameNoExt + "_updated.xml"));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void UpdateXMLClick(object sender, RoutedEventArgs e)
        {
            foreach (string filePath in selectedFilePaths)
            {
                UpdateXML(filePath);
            }

            System.Windows.Forms.MessageBox.Show("Finished processing.");
        }
    }
}
