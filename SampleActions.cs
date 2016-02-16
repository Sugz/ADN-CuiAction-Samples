#region Copyright
//      .NET Sample
//
//      Copyright (c) 2012 by Autodesk, Inc.
//
//      Permission to use, copy, modify, and distribute this software
//      for any purpose and without fee is hereby granted, provided
//      that the above copyright notice appears in all copies and
//      that both that copyright notice and the limited warranty and
//      restricted rights notice below appear in all supporting
//      documentation.
//
//      AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
//      AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
//      MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
//      DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
//      UNINTERRUPTED OR ERROR FREE.
//
//      Use, duplication, or disclosure by the U.S. Government is subject to
//      restrictions set forth in FAR 52.227-19 (Commercial Computer
//      Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
//      (Rights in Technical Data and Computer Software), as applicable.
//
#endregion

#region namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics; // useful for debugging...

// 3ds Max APIs
using ManagedServices;
// for 2012: using MaxCustomControls;
// for 2013:
using UiViewModels.Actions;
using Autodesk.Max;

#endregion

namespace AdnCuiSamples
{
    #region Using VS .NETTools
    // Regions are very useful to organize code blocks and to make code more readable.
    // using statements as seen above, simply provide an entry level to those name spaces. You can also alias them
    // like using Max = Autodesk.Max; Then all API access would be like Max.<API>
    //Tip: to implement required functionality, just right click to "Implement Abstract Class" to get stubs!
    //public class AdnCuiSample00 : CuiActionCommandAdapter
    //{
    //} 
    #endregion

    #region AbstractCustom_CuiActionCommandAdapter
    /// <summary>
    /// Tip create a abstract base from CuiActionCommandAdapter, 
    /// so you do not have to reimplement everything for each action.
    /// You can also directly implement the ICuiActionCommand interface on your own class.
    /// It is a bit more work, but gives you even more control.
    /// </summary>
    public abstract class AbstractCustom_CuiActionCommandAdapter : CuiActionCommandAdapter
    {
        public override string ActionText
        {
            get { return InternalActionText; }
        }

        public override string Category
        {
            get { return InternalCategory; }
        }

        public override string InternalActionText
        {
            get { return CustomActionText; }
        }

        public override string InternalCategory
        {
            get { return "ADN Samples"; }
        }

        public override void Execute(object parameter)
        {
            try
            {
                CustomExecute(parameter);
            }
            catch (System.Exception ex)
            {
                Debug.Print("Exception occurred: " + ex.Message);
            }
        }

        public abstract string CustomActionText { get; }
        public abstract void CustomExecute(object parameter);
    } 
    #endregion

    #region AdnCuiSample01
    /// <summary>
    /// Sample 1 simply gets access to the new API 
    /// and toggles a prompt in the status line.
    /// </summary>
    public class AdnCuiSample01 : AbstractCustom_CuiActionCommandAdapter
    {
        static bool bPrompt = true;
        MessageBoxResult showMessageBoxButton()
        {
            // Configure message box
            string message = "Should I toggle the prompt?";
            string caption = "Prompt Dialog";
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult defaultResult = MessageBoxResult.OK;
            // Show message box
            return MessageBox.Show(message, caption, buttons, icon, defaultResult);
        }

        public override string CustomActionText
        {
            get { return "01 - CallToAction :-)"; }
        }

        public override void CustomExecute(object parameter)
        {
            // Use the "Enchanced" Autodesk.Max APIs to Push/Pop a prompt to the UI
            IGlobal global = Autodesk.Max.GlobalInterface.Instance;
            IInterface14 ip = global.COREInterface14;

            MessageBoxResult result = showMessageBoxButton();
            if (result == MessageBoxResult.OK)
            {
                if (bPrompt)
                    ip.PushPrompt("Hello 3ds Max .Net API!");
                else
                    ip.PopPrompt();

                bPrompt = !bPrompt;
            }
        }
    } 
    #endregion

    #region AdnCuiSample02
    /// <summary>
    /// Sample 2 shows a simple window with XAML content. 
    /// One of the buttons creates a teapot in the scene wiht a modifier
    /// Another button will get render location and display the 
    /// last AVI file rendered.
    /// </summary>
    public class AdnCuiSample02 : AbstractCustom_CuiActionCommandAdapter
    {

        public override string CustomActionText
        {
            get { return "02 - WPF Quick Sample"; }
        }

        public override void CustomExecute(object parameter)
        {
            try
            {
                // Setup a dialog that is connected to the parent window. In this case
                // it is 3ds Max, so we use some of the utility APIs from ManagedServices
                // to make that connection.
                System.Windows.Window dialog = new System.Windows.Window();
                dialog.Title = "Sample Utility";
                dialog.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
                dialog.Content = new CreateTeapotUserControl1();
                dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                dialog.ShowInTaskbar = false;
                dialog.ResizeMode = System.Windows.ResizeMode.NoResize;

                System.Windows.Interop.WindowInteropHelper windowHandle =
                    new System.Windows.Interop.WindowInteropHelper(dialog);
                windowHandle.Owner = ManagedServices.AppSDK.GetMaxHWND();
                ManagedServices.AppSDK.ConfigureWindowForMax(dialog);

                dialog.Show();

            }
            catch (System.Exception ex)
            {
                Debug.Print("Exception occurred: " + ex.Message);
            }
        }
    } 
    #endregion

    #region AdnCuiSample03
    /// <summary>
    /// Sample 3 shows a dialog with XAML content that is hosted in the Viewport area
    /// This uses the MaxNotificationListener to handle the viewport messages
    /// so the dialog can follow the active viewport.
    /// The XAML background is transparent, and the controls have their opacity
    /// set less than 100% so the viewport geometry can show through.
    /// </summary>
    public class AdnCuiSample03 : AbstractCustom_CuiActionCommandAdapter
    {
        // Overrides for CuiActionCommandAdapter
        public override string CustomActionText
        {
            get { return "03 - WPF Viewport Sample"; }
        }

        public override void CustomExecute(object parameter)
        {
            try
            {
                // register only once
                if (bFirstTime)
                {
                    RegisterListeners();
                    bFirstTime = false;
                }

                // This allows the command to "toggle" the dialog on/off
                if (dialog == null)
                {
                    SetupDialog();
                    dialog.Show();
                }
                else
                {
                    dialog.Close();
                    dialog = null;
                }

                return;

            }
            catch (System.Exception ex)
            {
                Debug.Print("Exception occurred: " + ex.Message);
            }
        }

        // notification setup to allow XAML UI to follow the active viewport
        MaxNotificationListener ActiveVPListener
        {
            get;
            set;
        }

        MaxNotificationListener VPListener
        {
            get;
            set;
        }

        void ActiveVPListener_Handler(object sender, MaxNotificationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("active changed");

            CalculatePositionOfOverlay();
        }

        void VPListener_Handler(object sender, MaxNotificationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("layout changed");

            CalculatePositionOfOverlay();
        }

        public void RegisterListeners()
        {
            // Notice that these are notifiactions in .NET style and are from the ManagedServices assembly
            // Autodesk.Max also has notifications, but is using the C++ style callbacks with register/unregister mechanism
            ActiveVPListener = new ManagedServices.MaxNotificationListener((int)SystemNotificationCode.ActiveViewportChanged/*0x000000D8*/);
            ActiveVPListener.NotificationRaised += new EventHandler<MaxNotificationEventArgs>(ActiveVPListener_Handler);
            VPListener = new ManagedServices.MaxNotificationListener((int)SystemNotificationCode.ViewportChange/*0x00000003*/);
            VPListener.NotificationRaised += new EventHandler<MaxNotificationEventArgs>(VPListener_Handler);
        }

        // Calculate the dialog position relative to the active viewport
        void CalculatePositionOfOverlay()
        {

            if (null == dialog)
                return;
            IGlobal global = Autodesk.Max.GlobalInterface.Instance;
            IInterface14 ip = global.COREInterface14;

            // Use WpfUtilities to find active viewport coordinates. 
            // The activeviewport HWnd is obtained from Autodesk.Max API on interface14.
            // 2012: Rect windowRect = CSharpUtilities.WpfUtilities.GetNativeWindowRectInPixels(ip.ActiveViewport.HWnd);
            // 2013: 
            Rect windowRect = CSharpUtilities.WpfUtilities.GetNativeWindowRectInPixels(ip.ActiveViewExp.HWnd);
            dialog.Left = windowRect.Left;
            dialog.Top = windowRect.Top;

            dialog.Width = windowRect.Width;
            dialog.Height = windowRect.Height;

            // This helps to host it in the native application (3ds Max) by using the HWnd
            var helper = new System.Windows.Interop.WindowInteropHelper(dialog);
            // 2012: helper.Owner = ip.ActiveViewport.HWnd; 
            helper.Owner = ip.ActiveViewExp.HWnd; // ManagedServices.AppSDK.GetActiveViewportHwnd();
        }

        // Dailog setup code
        private bool bFirstTime = true;
        private System.Windows.Window dialog = null;
        // Setup and position the XAML based dialog 
        private void SetupDialog()
        {
            dialog = new System.Windows.Window();
            dialog.AllowsTransparency = true;
            dialog.Background = Brushes.Transparent;
            //dialog.Foreground = Brushes.Aqua;
            dialog.Opacity = 0.6;
            dialog.WindowStyle = System.Windows.WindowStyle.None;
            dialog.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            ViewportControl xamlControls = new ViewportControl();
            dialog.Content = xamlControls;
            dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            dialog.Left = 10.0;
            dialog.Top = 300.0;
            dialog.ShowInTaskbar = false;
            dialog.ResizeMode = System.Windows.ResizeMode.NoResize;
            dialog.RenderSize = new Size(300, 100);

            System.Windows.Interop.WindowInteropHelper windowHandle = new System.Windows.Interop.WindowInteropHelper(dialog);
            windowHandle.Owner = ManagedServices.AppSDK.GetMaxHWND();
            ManagedServices.AppSDK.ConfigureWindowForMax(dialog);

            xamlControls.ListAllNodes();

            CalculatePositionOfOverlay();
        }

    } 
    #endregion

    #region AdnCuiSample04
    /// <summary>
    /// Sample 4 shows how to instance geometry in a helical pattern
    /// See HelixControl.xaml and HelixControl.xaml.cs for logic.
    /// </summary>
    public class AdnCuiSample04 : AbstractCustom_CuiActionCommandAdapter
    {

        public override string CustomActionText
        {
            get { return "04 - Create Instances"; }
        }
        // Setup the XAML based dialog 
        private void SetupDialog()
        {
            try
            {

                System.Windows.Window dialog = new System.Windows.Window();
                dialog.Title = "Helix Instance";
                dialog.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
                dialog.Content = new HelixControl();
                dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                dialog.ShowInTaskbar = false;
                dialog.ResizeMode = System.Windows.ResizeMode.NoResize;

                System.Windows.Interop.WindowInteropHelper windowHandle =
                    new System.Windows.Interop.WindowInteropHelper(dialog);
                windowHandle.Owner = ManagedServices.AppSDK.GetMaxHWND();
                ManagedServices.AppSDK.ConfigureWindowForMax(dialog);

                dialog.Show();

            }
            catch (System.Exception ex)
            {
                Debug.Print("Exception occurred: " + ex.Message);
            }
        }

        public override void CustomExecute(object parameter)
        {
            try
            {
                SetupDialog();
            }
            catch (System.Exception ex)
            {
                Debug.Print("Exception occurred: " + ex.Message);
            }
        }
    } 
    #endregion

    #region AdnCuiSample05
    /// <summary>
    /// Sample 5 shows how get an objects state and iterate it's mesh
    /// to create new individual face objects
    /// </summary>
    public class AdnCuiSample05 : AbstractCustom_CuiActionCommandAdapter
    {

        public override string CustomActionText
        {
            get { return "05 - Create Face Objects"; }
        }

        public override void CustomExecute(object parameter)
        {
            try
            {

                IGlobal global = Autodesk.Max.GlobalInterface.Instance;
                IInterface14 ip = global.COREInterface14;

                // Get the first selected node...
                IINode node = ip.GetSelNode(0);

                // Get it's current object state. If a modifier has been applied, for example,
                // it is going to return the OS of the mesh in it's current form in the timeline.
                IObjectState os = node.ObjectRef.Eval(ip.Time);

                // Now grab the object itself.
                IObject objOriginal = os.Obj;

                // Let's make sure it is a TriObject, which is the typical kind of object with a mesh
                if (!objOriginal.IsSubClassOf(global.TriObjectClassID))
                {
                    // If it is NOT, see if we can convert it...
                    if (objOriginal.CanConvertToType(global.TriObjectClassID) == 1)
                        objOriginal = objOriginal.ConvertToType(ip.Time, global.TriObjectClassID);
                    else
                        return;
                }

                // Store the orginal transform positioning data
                IMatrix3 mat = node.GetNodeTM(0, null);
                IPoint3 ptOffsetPos = node.ObjOffsetPos;
                IQuat quatOffsetRot = node.ObjOffsetRot;
                IScaleValue scaleOffsetScale = node.ObjOffsetScale;

                // Now we should be safe to know it is a TriObject and we can cast it as such.
                // An exception will be thrown...
                ITriObject triOriginal = objOriginal as ITriObject;


                // Let's first setup a class ID for the type of objects are are creating.
                // New TriObject in this case to hold each face.
                IClass_ID cid = global.Class_ID.Create((uint)BuiltInClassIDA.TRIOBJ_CLASS_ID, 0);

                // We can grab the faces as a List and iterate them in .NET API.
                IList<IFace> faces = triOriginal.Mesh.Faces;
                foreach (IFace face in faces)
                {
                    // Create a new TriObject for each new face.
                    object objectNewFace = ip.CreateInstance(SClass_ID.Geomobject, cid as IClass_ID);

                    // Create a new node to hold it in the scene.
                    IObject objNewFace = (IObject)objectNewFace;
                    IINode n = global.COREInterface.CreateObjectNode(objNewFace);

                    // Name it and ensure it is unique...
                    string newname = "ADN-Sample-Face";
                    ip.MakeNameUnique(ref newname);
                    n.Name = newname;

                    // Based on what we created above, we can safely cast it to TriObject
                    ITriObject triNewFace = objNewFace as ITriObject;

                    // Setup the new TriObject with 1 face, and the vertex count from the original object's face we are processing
                    triNewFace.Mesh.SetNumFaces(1, false, false);
                    triNewFace.Mesh.SetNumVerts(face.V.Count(), false, false);

                    // Finish setting up the face (always face '0' because there will only be one per object).
                    triNewFace.Mesh.Faces[0].SetVerts(0, 1, 2);
                    triNewFace.Mesh.Faces[0].SetEdgeVisFlags(EdgeVisibility.Vis, EdgeVisibility.Vis, EdgeVisibility.Vis);
                    triNewFace.Mesh.Faces[0].SmGroup = 2;

                    // Now, for each vertex, get the old face's points and store into new.
                    for (int i = 0; i < face.V.Count(); i++)
                    {
                        //Get the vertex from the original object's face we are processing
                        IPoint3 point = triOriginal.Mesh.GetVert((int)face.GetVert(i));
                        // Set the vertex point in the new face vertex
                        triNewFace.Mesh.SetVert(i, point);
                    }

                    // update transform to match object being exploded.
                    n.SetNodeTM(0, mat);
                    n.ObjOffsetPos = ptOffsetPos;
                    n.ObjOffsetRot = quatOffsetRot;
                    n.ObjOffsetScale = scaleOffsetScale;
                    n.ObjOffsetPos = ptOffsetPos;
                    n.CenterPivot(0, false);

                    // make it draw.
                    triNewFace.Mesh.InvalidateGeomCache();
                }


            }
            catch (System.Exception ex)
            {
                Debug.Print("Exception occurred: " + ex.Message);
            }
        }

        private void CreateFacesFromNode(IINode node)
        {


        }
    }
    
    #endregion
}
