using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using Autodesk.Max;

namespace AdnCuiSamples
{

	/// <summary>
    /// Interaction logic for CreateTeapotUserControl1.xaml
	/// </summary>
	public partial class CreateTeapotUserControl1 : UserControl
	{
        bool bPlay = false;

        /// <summary>
        /// Initialize the WPF panel
        /// </summary>
		public CreateTeapotUserControl1()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Play a video just to show some of the nice XAML features
        /// Gets the video by using the 3ds Max APIs to obtain the last
        /// rendered AVI file of an example where you might use WPF
        /// with 3ds Max APIs
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // toggle to state.
                bPlay = !bPlay;

                if (bPlay)
                {
                    // here we can use the PathConfigMgr class to get the render output directory.
                    IGlobal global = GlobalInterface.Instance;
                    string pathRenderOutput = global.IPathConfigMgr.PathConfigMgr.GetDir(MaxDirectory.RenderOutput);

                    // Convert the string value into something .NET likes.
                    var directory = new DirectoryInfo(pathRenderOutput);

                    // Now we can use Linq to conviently search the directory for the latest AVI file present.
                    // avi files work great, so for example purposes, we are using only that type.
                    var maxRenderFile = (from f in directory.GetFiles("*.avi")
                                  orderby f.LastWriteTime descending
                                  select f).First();

                    // quick check to determine if we have something we can display...
                    if (maxRenderFile != null)
                    {
                        System.Uri uri = new Uri(maxRenderFile.FullName);
                        mediaTimelineMaxRenderFile.Source = uri;
                    }

                    // Make it visible to see on the dialog. The XAML property sets it to be 90% opaque,
                    // so the background will show through.
                    mediaElementMaxRenderFile.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    // toggle it back off again.
                    mediaElementMaxRenderFile.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch (System.Exception ex) // note this is not recommended for production apps. Just for debugging and testing
                                        // to see what true exceptions you need to handle.
            {
                Debug.Print(ex.Message);
            }
        }

        /// <summary>
        /// Use the "Enchanced" Autodesk.Max APIs to Create a teapot
        /// </summary>
         private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IGlobal global = GlobalInterface.Instance;
                IInterface14 coreInterface = global.COREInterface14;
                
                // Push the prompt... This is not so important in this example,
                // but to show how it works in case you might have a lengthy operation
                // and want to show a prompt during. Can also be useful to ask user
                // to select something, etc.
                coreInterface.PushPrompt("Creating teapot!");

                // Must use Class ids with the enhanced API. This is where is can be useful to also
                // have the C++ SDK installed to use as examples.
                IClass_ID cidTeapot = global.Class_ID.Create((uint)BuiltInClassIDA.TEAPOT_CLASS_ID,
                                                             (uint)BuiltInClassIDB.TEAPOT_CLASS_ID);
                
                // Construct the teapot. Note it's super class id is Geometry object
                IObject objTeapot = coreInterface.CreateInstance(SClass_ID.Geomobject, cidTeapot) as IObject;

                // Create a new node for the teapot to use. This is necessary to get the 
                // teapot to show in the scene. All objects should have a node within the scene.
                IINode node = coreInterface.CreateObjectNode(objTeapot);

                // This shows more cross-over with the C++ SDK
                // In this case the teapot is using the param array
                // which is an older form of object data. Newer objects use ParamBlock2.
                // By looking at the C++ side, you can find that the parameter at index zero
                // is the size for the teapot. Other paramneters are indexed, too.
                IIParamArray pb = objTeapot.ParamBlock;
                pb.SetValue(0, coreInterface.Time, 20.0f); // teapot size is at index zero.

                // This will move it to a location in space other than origin.
                IMatrix3 m = global.Matrix3.Create();
                m.IdentityMatrix();
                node.Move(coreInterface.Time, m,
                            // Move 10 units in x
                            global.Point3.Create(10, 0, 0), true, true, 0, true);

                // Now let's also add a modifier to the object's stack.
                // We will use the bend modifier, note that is uses only part A of the classid, 
                // but both parts must be present. This is just sett9ng up the class id.
                IClass_ID cidOsmBend = global.Class_ID.Create((uint)BuiltInClassIDA.BENDOSM_CLASS_ID,
                                                              (uint)0);
                // Here we can pull the object back from the node. This is not entirely 
                // necessary since we created it above, but this shows a technique for already existing objects.
                IObject obj = node.ObjectRef;

                // Modifiers require a "derived object" to setup the geomtry pipeline.
                // Do not confuse it with the programming term for "derived".
                IIDerivedObject dobj = global.CreateDerivedObject(obj);                
                
                // Now use the class id to create the modifier itself.
                object objMod = coreInterface.CreateInstance(SClass_ID.Osm, cidOsmBend as IClass_ID);
                IModifier mod = (IModifier)objMod;

                // Finally connect it back to the object's stack. Here we use the derived 
                // object constructed above, to add the modifier to.
                dobj.AddModifier(mod, null, 0); // top of stack
                // Then assign it back to the node as the current object reference.
                node.ObjectRef = dobj;
                
                // Now setup the modifier with some data. In thise case it uses ParamBlock2
                IIParamBlock2 pbBend = mod.GetParamBlock(0);
                pbBend.SetValue(0, 0, 30.0f, 0); // bend angle parameter is at index zero of the parameter block.

                // now update all the viewports. Note this can be expensive, so use it at the end typically.
                coreInterface.RedrawViewportsNow(0, 0);
                
                // clear the prompt.
                coreInterface.PopPrompt();
            }
            catch (System.Exception ex) 
            // note this is not recommended for production apps. Just for debugging and testing
            // to see what true exceptions you need to handle.
            {
                Debug.Print(ex.Message);
            }
        }
	}
}
