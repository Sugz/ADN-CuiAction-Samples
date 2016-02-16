using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Autodesk.Max;

namespace AdnCuiSamples
{
	/// <summary>
    /// Interaction logic for ViewportControl.xaml
	/// </summary>
	public partial class ViewportControl
	{
        bool bError = false;
        bool bDisplayMax = true;

		public ViewportControl()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Button handler to List all the nodes in the scene
        /// </summary>
        private void buttonAllNodes_Click(object sender, RoutedEventArgs e)
        {
            ListAllNodes();
        }

        /// <summary>
        /// Button handler to List the lights
        /// </summary>
        private void buttonLights_Click(object sender, RoutedEventArgs e)
        {
            ListLightNodes();
        }

        /// <summary>
        /// Button handler to Select the nodes that are selected in the list control
        /// </summary>
        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            StringCollection selected = new StringCollection();
            foreach (string item in listSceneNodes.SelectedItems)
                selected.Add(item);
            SelectNodes(selected);
        }

        /// <summary>
        /// Button handler to Clear the node selection
        /// </summary>
        private void buttonClearSelect_Click(object sender, RoutedEventArgs e)
        {
            listSceneNodes.SelectedIndex = -1;
            IGlobal Global = Autodesk.Max.GlobalInterface.Instance;
            IInterface14 Interface = Global.COREInterface14;
            Interface.ClearNodeSelection(false);
        }

        /// <summary>
        /// Button handler to Show/Hide the form in the viewport.
        /// </summary>
        private void buttonMinMax_Click(object sender, RoutedEventArgs e)
        {
            if (bDisplayMax)
            {
                listSceneNodes.Visibility = System.Windows.Visibility.Hidden;
                buttonAllNodes.Visibility = System.Windows.Visibility.Hidden;
                buttonLights.Visibility = System.Windows.Visibility.Hidden;
                buttonSelectNode.Visibility = System.Windows.Visibility.Hidden;
                buttonClearSelection.Visibility = System.Windows.Visibility.Hidden;
                buttonMinMax.Content = "+";
            }
            else
            {
                listSceneNodes.Visibility = System.Windows.Visibility.Visible;
                buttonAllNodes.Visibility = System.Windows.Visibility.Visible;
                buttonLights.Visibility = System.Windows.Visibility.Visible;
                buttonSelectNode.Visibility = System.Windows.Visibility.Visible;
                buttonClearSelection.Visibility = System.Windows.Visibility.Visible;
                buttonMinMax.Content = "-";
            }
            bDisplayMax = !bDisplayMax;
        }

        // Data...
        List<IINode> m_sceneNodes = new List<IINode> { };

        /// <summary>
        /// Recursively go through the scene and get all nodes
        /// Use the "Enchanced" Autodesk.Max APIs to get the children nodes
        /// </summary>
        private void GetNodes(IINode node)
        {
            m_sceneNodes.Add(node);

            for (int i = 0; i < node.NumberOfChildren; i++)
                GetNodes(node.GetChildNode(i));
        }

        /// <summary>
        /// Start recursion of scene at the root node
        /// Then add the nodes to the list control
        /// Use the "Enchanced" Autodesk.Max APIs to get the root node
        /// </summary>
        public void ListAllNodes()
        {

            m_sceneNodes.Clear();

            IGlobal Global = Autodesk.Max.GlobalInterface.Instance;
            IInterface14 Interface = Global.COREInterface14;
            if (bError) 
                Interface.PopPrompt();

            // Start with the root node
            IINode nodeRoot = Interface.RootNode;
            GetNodes(nodeRoot);

            listSceneNodes.Items.Clear();
            foreach (IINode item in m_sceneNodes)
            {
                listSceneNodes.Items.Add(item.Name);
            }
        }

        /// <summary>
        /// Use LINQ (Language Integrated Query) 
        /// along with the "Enchanced" Autodesk.Max APIs to find objects of ILightObject type
        /// </summary>
        private void ListLightNodes()
        {
            IGlobal Global = Autodesk.Max.GlobalInterface.Instance;
            IInterface14 Interface = Global.COREInterface14;
            if (bError)
                Interface.PopPrompt();

            // Use LINQ to filter for lights!
            var sceneLights = from nodeLight in m_sceneNodes
                         where nodeLight.ObjectRef is ILightObject
                         select nodeLight;

            // clear the dialog list
            listSceneNodes.Items.Clear();
            
            // Update both lists
            foreach (IINode item in sceneLights)
            {
                listSceneNodes.Items.Add(item.Name);
            }

        }

        /// <summary>
        /// Use LINQ (Language Integrated Query) 
        /// along with the "Enchanced" Autodesk.Max APIs to
        /// make a new list of "selected" nodes for eventual selection in the scene.
        /// </summary>
        private void SelectNodes(StringCollection nodeNames)
        {
            IGlobal Global = Autodesk.Max.GlobalInterface.Instance;
            IInterface14 Interface = Global.COREInterface14;
            if (bError)
                Interface.PopPrompt();

            if (nodeNames.Contains("Scene Root"))
            {
                Interface.PushPrompt("Cannot select root node");
                bError = true;
                return;
            }

            IEnumerable<IINode> nodesSelected =
                                     from node in m_sceneNodes
                                     where nodeNames.Contains(node.Name)
                                     select node;
            
            Interface.ClearNodeSelection(false);

            foreach (IINode node in nodesSelected)
                Interface.SelectNode(node, false);
        }

	}
}