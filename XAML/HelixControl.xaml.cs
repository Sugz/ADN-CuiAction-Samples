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
	/// Interaction logic for HelixControl.xaml
	/// </summary>
	public partial class HelixControl : UserControl
	{
        public HelixControl()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Create the instances in a helical pattern
        /// </summary>
        private void buttonCreateInst_Click(object sender, RoutedEventArgs e)
        {
            double dHelixDist;
            double dRadius;
            int nNumInst;
            int nNumRevs;

            // Get the data from the UI text boxes. 
            // Some error checking, but not much. :-)
            try
            {
                dHelixDist = Convert.ToDouble(textBoxHelixDistance.Text);
                dRadius = Convert.ToDouble(textBoxRadius.Text);
                nNumInst = Convert.ToInt32(textBoxNumberInst.Text);
                nNumRevs = Convert.ToInt32(textBoxNumberRevs.Text);

            }
            catch (FormatException) // make sure the numbers were converted
            {
                return;
            }
            catch (OverflowException) // check for overflow
            {
                return;
            }
            catch (SystemException) // anything else wrong? Also exit.
            {
                return;
            }

            // Max API access
            IGlobal global = Autodesk.Max.GlobalInterface.Instance;
            IInterface14 ip = global.COREInterface14;

            // Get the first selected node...
            IINode node = ip.GetSelNode(0);

            if (node == null)
                return;

            // Setup a node table (special array of nodes required for certain APIs).
            // In this case it is just one item, but you can see how you might do 
            // the same operation on the entire seletion set for example.
            IINodeTab tabSource = global.NodeTab.Create();
            tabSource.AppendNode(node, false, 0);

            // Setup some pattern values
            double dDegreesOfMovement = 2.0 * Math.PI;
            double dIncrement = dDegreesOfMovement / nNumInst;

            // Get the node's position
            IInterval intv = global.Interval.Create();
            intv.SetInfinite();
            IPoint3 center = node.GetNodeTM(0, intv).Trans;
            
            double x = 0.0;
            double y = 0.0;
            double z = center.Z;

            // using the input values, create instanced nodes
            // Not terribly robust, but shows the idea
            // as there is no concern for coordinate systems, etc. Just assumes
            // the source object is in basic world coordinates to start.
            for (int n = 0; n < nNumRevs; n++)
            {
                for (double dAngle = 0.0; dAngle < dDegreesOfMovement; dAngle+=dIncrement)
                {
             		x = center.X + (dRadius * Math.Sin(dAngle));
                    y = center.Y + (dRadius * Math.Cos(dAngle));
                    z += dHelixDist;

                    // Create these locally because each one should be intialized.
                    // GC will clean them as necessary.
                    IPoint3 point = global.Point3.Create(x, y, z);
                    IINodeTab tabResultSource = global.NodeTab.Create();
                    IINodeTab tabResultTarget = global.NodeTab.Create(); ;
                    // This API is a "do all", so we have to understand it... The last two parameters are "out"
                    // but have to be allocated to start (as above) making them in/out in nature.
                    // Basically the resultSource contains the nodes that were used for the "clone". Because
                    // the base table could have nodes with children, the resultSource could have more
                    // that was provided.
                    ip.CloneNodes(tabSource, point, false, CloneType.Instance, tabResultSource, tabResultTarget);

                    // tabResultTarget contains the new node(s). In this case there should be only one, since we
                    // started with only one, but depending on what you select there could be dependents. This
                    // sample code does not handle that more complex situation, but be aware. :-)
                    IINode nodeR = tabResultTarget[(IntPtr)0];
                    intv.SetInfinite();

                    // Get the nodes transformation matrix...
                    IMatrix3 m = nodeR.GetNodeTM(0, intv);
                    // rotate it by the angle
                    m.RotateZ((float)dAngle);
                    // move it to the new point in the helical path
                    m.Translate(point);
                    // then we have to apply it back to the node.
                    nodeR.SetNodeTM(0, m);
                }
            }

        }
    }
}