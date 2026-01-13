
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System;
using System.Windows.Media.Media3D;
using System.Transactions;
using System.Windows.Controls;
using System.Collections.Generic;

class Scena
{
    Viewport3D viewPort3D;
    List<Model3D> elementeScenaModel3D;
    List<ElementScena> listaElementeScena;
    //===================================
    private Model3DGroup MainModel3Dgroup;
    // Lumini
    PointLight point_light;
    AmbientLight ambient_light;
    DirectionalLight directional_light;
    SpotLight spot_light;
    // Camera.
    private PerspectiveCamera TheCamera;

    private double CameraR = 100, CameraPhi = Math.PI / 4, CameraTheta = -3 * Math.PI / 4;//pentru pozitia camerei in jurul punctului la care se uita
    private const double CameraDPhi = Math.PI / 180;
    private const double CameraDTheta = Math.PI / 180;
    private const double CameraDR = 100;

    //===================================
    public Scena(Viewport3D viewPort3D)
    {
        this.viewPort3D = viewPort3D;
        elementeScenaModel3D = new List<Model3D>();
        listaElementeScena= new List<ElementScena>();
        //===============================================
        MainModel3Dgroup = new Model3DGroup();
        // Pozitia initiala a camerei
        TheCamera = new PerspectiveCamera();
        TheCamera.FieldOfView = 45;
        TheCamera.FarPlaneDistance = 300;
        TheCamera.NearPlaneDistance = 1;

        viewPort3D.Camera = TheCamera;
        DefineLights();
    }
    public void afiseaza_elemente()
    {
        elementeScenaModel3D.Clear();
        viewPort3D.Children.Clear();

        MainModel3Dgroup = new Model3DGroup();
        TheCamera = new PerspectiveCamera();
        TheCamera.FieldOfView = 60;
        viewPort3D.Camera = TheCamera;
        DefineLights();
        PositionCamera();
        //adaugam luminile
        MainModel3Dgroup.Children.Add(ambient_light);
        MainModel3Dgroup.Children.Add(directional_light);
        MainModel3Dgroup.Children.Add(point_light);
        MainModel3Dgroup.Children.Add(spot_light);
        //adaugam elementele scenei
        foreach (ElementScena es in listaElementeScena)
        {
            MainModel3Dgroup.Children.Add(es.DaModel3D());
        }
        ModelVisual3D model_visual = new ModelVisual3D();
        model_visual.Content = MainModel3Dgroup;
        viewPort3D.Children.Add(model_visual);
    }

    private void DefineLights()
    {
        // Definim luminile
        ambient_light = new AmbientLight(Color.FromArgb(255, 100, 100, 100));
        directional_light = new DirectionalLight(Color.FromArgb(255, 55, 55, 55), new Vector3D(0, -1, -1));
        point_light = new PointLight(Color.FromArgb(0, 0, 0, 0), new Point3D(0, 0, 0));

        spot_light = new SpotLight();
        spot_light.Color = Color.FromArgb(255, 100, 100, 100);
        spot_light.Position = new Point3D(3, 3, 20);
        spot_light.Direction = new Vector3D(0, 0, -1);
        spot_light.InnerConeAngle = 30;
        spot_light.OuterConeAngle = 60;
        spot_light.Range = 50;
    }
    private void PositionCamera()
    {
        //calculam pozitia camerei in coordonate Carteziene
        double y = CameraR * Math.Sin(CameraPhi);
        double hyp = CameraR * Math.Cos(CameraPhi);
        double x = hyp * Math.Cos(CameraTheta);
        double z = hyp * Math.Sin(CameraTheta);
        TheCamera.Position = new Point3D(x, y, z);
        // orientam camera spre origine
        TheCamera.LookDirection = new Vector3D(-x, -y, -z);
        // setam orientarea camerei
        TheCamera.UpDirection = new Vector3D(0, 1, 0);
    }
    public void modifica_Phi(double phi)
    {
        CameraPhi = phi;
        // actualizam pozitia camerei
        PositionCamera();
    }
    public void modifica_Theta(double theta)
    {
        CameraTheta = theta;
        // actualizam pozitia camerei
        PositionCamera();
    }
    public void modifica_R(double r)
    {
        CameraR = r;
        // actualizam pozitia camerei
        PositionCamera();
    }
    public void apropie_camera()
    {
        CameraR -= CameraDR;
        if (CameraR < CameraDR) CameraR = CameraDR;
        // actualizam pozitia camerei
        PositionCamera();
    }
    public void departeaza_camera()
    {
        CameraR += CameraDR;
        // actualizam pozitia camerei
        PositionCamera();
    }

    //=========================================
    public void valoare_lumina(byte valoare)
    {
        point_light.Color = Color.FromArgb(0, valoare, valoare, 0);
    }
    public void adauga_element_scena(ElementScena es)
    {
        listaElementeScena.Add(es);
    }
}

public interface ElementScena
{
    public Model3DGroup DaModel3D();
}
class Pereti_ext : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public Pereti_ext(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\perete.jpg", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();
        adaugaTriunghi(mesh, new Point3D(-1, -1, 1), new Point3D(1, -1, 1), new Point3D(-1, 1, 1), new Point(0, 1), new Point(1, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, 1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1), new Point(1, 1), new Point(1, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(-1, 1, -1), new Point(0, 1), new Point(1, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(-1, 1, -1), new Point(1, 1), new Point(1, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), new Point3D(-1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), new Point3D(-1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(-1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        //capatul de langa acoperis
        adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(-1, 1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class Copac : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public Copac(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\Copac.jpg", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();

        //tulpina
        adaugaTriunghi(mesh, new Point3D(-1, -1, 2), new Point3D(1, -1, 2), new Point3D(-1, 1, 2), new Point(0, 1), new Point(0.5, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, 2), new Point3D(1, 1, 2), new Point3D(-1, 1, 2), new Point(0.5, 1), new Point(0.5, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(1, 1, 2), new Point3D(2, 1, 1), new Point3D(2, -1, 1), new Point(0, 1), new Point(0.5, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, 2), new Point3D(1, 1, 2), new Point3D(2, -1, 1), new Point(0.5, 1), new Point(0.5, 0), new Point(0, 0));


        adaugaTriunghi(mesh, new Point3D(2, -1, -1), new Point3D(2, 1, -1), new Point3D(2, -1, 1), new Point(0, 0), new Point(0, 1), new Point(0.5, 0));
        adaugaTriunghi(mesh, new Point3D(2, 1, -1), new Point3D(2, 1, 1), new Point3D(2, -1, 1), new Point(0, 1), new Point(0.5, 1), new Point(0.5, 0));

        adaugaTriunghi(mesh, new Point3D(1, -1, -2), new Point3D(1, 1, -2), new Point3D(2, 1, -1), new Point(0, 0), new Point(0, 1), new Point(0.5, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, -2), new Point3D(2, 1, -1), new Point3D(2, -1, -1), new Point(0, 1), new Point(0.5, 1), new Point(0.5, 0));


        adaugaTriunghi(mesh, new Point3D(-1, -1, -2), new Point3D(1, -1, -2), new Point3D(-1, 1, -2), new Point(0, 1), new Point(0.5, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, -2), new Point3D(1, 1, -2), new Point3D(-1, 1, -2), new Point(0.5, 1), new Point(0.5, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -2), new Point3D(-2, -1, -1), new Point3D(-1, 1, -2), new Point(0, 1), new Point(0.5, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(-2, -1, -1), new Point3D(-2, 1, -1), new Point3D(-1, 1, -2), new Point(0.5, 1), new Point(0.5, 0), new Point(0, 0));


        adaugaTriunghi(mesh, new Point3D(-2, -1, -1), new Point3D(-2, 1, -1), new Point3D(-2, -1, 1), new Point(0, 0), new Point(0, 1), new Point(0.5, 0));
        adaugaTriunghi(mesh, new Point3D(-2, 1, -1), new Point3D(-2, 1, 1), new Point3D(-2, -1, 1), new Point(0, 1), new Point(0.5, 1), new Point(0.5, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, 2), new Point3D(-2, 1, 1), new Point3D(-2, -1, 1), new Point(0, 1), new Point(0.5, 1), new Point(0.5, 0));
        adaugaTriunghi(mesh, new Point3D(-1, -1, 2), new Point3D(-2, 1, 1), new Point3D(-1, 1, 2), new Point(0, 1), new Point(0.5, 1), new Point(0.5, 0));

        //----coroana

        adaugaTriunghi(mesh, new Point3D(-3, 1, 3), new Point3D(3, 1, 3), new Point3D(-3, 4, 3), new Point(0.5, 1), new Point(1, 1), new Point(0.5, 0));
        adaugaTriunghi(mesh, new Point3D(3, 1, 3), new Point3D(3, 4, 3), new Point3D(-3, 4, 3), new Point(1, 1), new Point(1, 0), new Point(0.5, 0));

        adaugaTriunghi(mesh, new Point3D(-3, 1, -3), new Point3D(3, 1, -3), new Point3D(-3, 4, -3), new Point(0.5, 1), new Point(1, 1), new Point(0.5, 0));
        adaugaTriunghi(mesh, new Point3D(3, 1, -3), new Point3D(3, 4, -3), new Point3D(-3, 4, -3), new Point(1, 1), new Point(1, 0), new Point(0.5, 0));

        adaugaTriunghi(mesh, new Point3D(3, 1, -3), new Point3D(3, 4, -3), new Point3D(3, 1, 3), new Point(0.5, 0), new Point(0.5, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(3, 4, -3), new Point3D(3, 4, 3), new Point3D(3, 1, 3), new Point(0.5, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-3, 1, -3), new Point3D(-3, 4, -3), new Point3D(-3, 1, 3), new Point(0.5, 0), new Point(0.5, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(-3, 4, -3), new Point3D(-3, 4, 3), new Point3D(-3, 1, 3), new Point(0.5, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-3, 1, -3), new Point3D(3, 1, -3), new Point3D(-3, 1, 3), new Point(0.5, 0), new Point(0.5, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(3, 1, -3), new Point3D(3, 1, 3), new Point3D(-3, 1, 3), new Point(0.5, 1), new Point(1, 1), new Point(1, 0));


        adaugaTriunghi(mesh, new Point3D(-3, 4, -3), new Point3D(3, 4, -3), new Point3D(-3, 4, 3), new Point(0.5, 0), new Point(0.5, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(3, 4, -3), new Point3D(3, 4, 3), new Point3D(-3, 4, 3), new Point(0.5, 1), new Point(1, 1), new Point(1, 0));






        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class Gazon : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public Gazon(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\iarba.jpg", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();
        adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(-1, 1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1), new Point(0,1), new Point(1, 1), new Point(1, 0));

        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class Geam : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public Geam(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\geam.png", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), new Point3D(-1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), new Point3D(-1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class usaGaraj : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public usaGaraj(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\Acoperis.jpg", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();
       
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), new Point3D(-1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), new Point3D(-1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));


        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class Usa : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public Usa(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\usa.jpg", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), new Point3D(-1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), new Point3D(-1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class Pereti_int : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public Pereti_int(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\interior.jpg", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();
        adaugaTriunghi(mesh, new Point3D(-1, -1, 1), new Point3D(1, -1, 1), new Point3D(-1, 1, 1), new Point(0, 1), new Point(1, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, 1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1), new Point(1, 1), new Point(1, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(-1, 1, -1), new Point(0, 1), new Point(1, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(-1, 1, -1), new Point(1, 1), new Point(1, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), new Point3D(-1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), new Point3D(-1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(-1, -1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        //capatul de langa acoperis
        //adaugaTriunghi(mesh, new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(-1, 1, 1), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        //adaugaTriunghi(mesh, new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class Acoperis : ElementScena
{
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public Acoperis(Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        ImageBrush textura = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\cosmi\OneDrive\Desktop\3Dhouse\Grafica 3D\img\Acoperis.jpg", UriKind.RelativeOrAbsolute)));
        textura.Viewport = new Rect(0, 0, 1, 1);
        textura.TileMode = TileMode.Tile;
        textura.ViewportUnits = BrushMappingMode.Absolute;//setam modul Absolute

        //cream suprafata.
        MeshGeometry3D mesh = new MeshGeometry3D();
        adaugaTriunghi(mesh, new Point3D(1, -1, -1), new Point3D(1, -1, 1), new Point3D(0, 1, -1), new Point(0, 1), new Point(1, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(1, -1, 1), new Point3D(0, 1, 1), new Point3D(0, 1, -1), new Point(1, 1), new Point(1, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, -1), new Point3D(-1, -1, 1), new Point3D(0, 1, -1), new Point(0, 1), new Point(1, 1), new Point(0, 0));
        adaugaTriunghi(mesh, new Point3D(-1, -1, 1), new Point3D(0, 1, 1), new Point3D(0, 1, -1), new Point(1, 1), new Point(1, 0), new Point(0, 0));

        adaugaTriunghi(mesh, new Point3D(-1, -1, 0.85), new Point3D(0, 1, 0.85), new Point3D(1, -1, 0.85), new Point(0, 0), new Point(0, 1), new Point(1, 0));
        adaugaTriunghi(mesh, new Point3D(-1, -1, -0.85), new Point3D(0, 1, -0.85), new Point3D(1, -1, -0.85), new Point(0, 1), new Point(1, 1), new Point(1, 0));

        
        // Coloram suprafata
        DiffuseMaterial surface_material = new DiffuseMaterial(textura);
        /*DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.Orange);*/

        GeometryModel3D geometryModel3D = new GeometryModel3D(mesh, surface_material);

        // facem suprafata vizibila pe ambele fete
        geometryModel3D.BackMaterial = surface_material;

        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();

        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);

        geometryModel3D.Transform = transformGroup;

        Model3DGroup model3DGroup = new Model3DGroup();
        model3DGroup.Children.Add((Model3D)geometryModel3D);
        return model3DGroup;
    }
    private void adaugaTriunghi(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3, Point pT1, Point pT2, Point pT3)
    {
        //adaugam punctele
        mesh.Positions.Add(p1);
        mesh.Positions.Add(p2);
        mesh.Positions.Add(p3);

        mesh.TextureCoordinates.Add(pT1);//3
        mesh.TextureCoordinates.Add(pT2);//0
        mesh.TextureCoordinates.Add(pT3);//1


        //adaugam triunghiul format de cele trei puncte
        int nr = mesh.Positions.Count - 1;
        mesh.TriangleIndices.Add(nr);
        mesh.TriangleIndices.Add(nr - 1);
        mesh.TriangleIndices.Add(nr - 2);
    }
}
class ObiectXAML : ElementScena
{
    string resourceKey;
    Vector3D vector3D;
    double alpha;
    Vector3D indZoom;
    public ObiectXAML(string resourceKey, Vector3D vector3D, double alpha, Vector3D indZoom)
    {
        this.resourceKey = resourceKey;
        this.vector3D = vector3D;
        this.alpha = alpha;
        this.indZoom = indZoom;
    }
    public Model3DGroup DaModel3D()
    {
        Model3DGroup model3DGroup = new Model3DGroup();
        Model3DGroup tempModel3DGroup = Application.Current.Resources[resourceKey] as Model3DGroup;
        model3DGroup.Children.Add(tempModel3DGroup);


        //definim transformarile pe baza parametrilor
        Transform3DGroup transformGroup = new Transform3DGroup();
        // Translatie
        TranslateTransform3D translate_transform = new TranslateTransform3D(vector3D);
        // Rotatie in jurul unui vector dat, de unghi dat
        RotateTransform3D rotate_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), alpha));
        // Omotetie
        ScaleTransform3D scaleTransform = new ScaleTransform3D(indZoom);

        transformGroup.Children.Add(rotate_transform);
        transformGroup.Children.Add(scaleTransform);
        transformGroup.Children.Add(translate_transform);


        model3DGroup.Transform = transformGroup;
        return model3DGroup;
    }
}
