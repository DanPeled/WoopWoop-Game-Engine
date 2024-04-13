using WoopWoop;
using WoopWoop.UI;
using ZeroElectric.Vinculum;

public class UITesting : Game
{
    Entity slider = new();
    public override void Start()
    {
        RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiControlProperty.BORDER_COLOR_NORMAL, 255);
        RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiControlProperty.TEXT_COLOR_NORMAL, 0xfffffff);
        RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiControlProperty.BASE_COLOR_FOCUSED, 0x383333ff);
        RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiControlProperty.BASE_COLOR_PRESSED, 0x383333ff);
        Camera.Main().backgroundColor = Raylib.GRAY;
        RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiControlProperty.BASE_COLOR_NORMAL, 0x2c3334ff);
        RayGui.GuiSetStyle((int)GuiControl.COLORPICKER, (int)GuiControlProperty.BORDER_COLOR_NORMAL, 255);
        RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiControlProperty.BORDER_WIDTH, 2);
        RayGui.GuiSetStyle((int)GuiControl.BUTTON, (int)GuiControlProperty.BORDER_WIDTH, 2);
        RayGui.GuiSetStyle((int)GuiControl.SLIDER, (int)GuiControlProperty.BASE_COLOR_PRESSED, 0xfffffff);



        slider.transform.Scale = new(200, 20);
        slider.transform.Position = new(10, 10);
        slider.AddComponent<Slider>();
        Entity.Instantiate(slider);


        Entity button = new(10, 40);
        button.transform.Scale = new(200, 200);
        button.AddComponent<Button>();
        Entity.Instantiate(button);

        Entity sliderbar = new();
        sliderbar.transform.Scale = new(200, 20);
        sliderbar.transform.Position = new(220, 10);
        sliderbar.AddComponent<SliderBar>();
        Entity.Instantiate(sliderbar);

        // Entity textInput = Entity.CreateEntity().AddComponent<TextInput>().SetScale(300, 300).Create();
        // Entity.Instantiate(textInput);

        Entity colorpicker = new();
        colorpicker.transform.Scale = new(200, 200);
        colorpicker.transform.Position = new(220, 40);
        colorpicker.AddComponent<ColorPicker>();
        Entity.Instantiate(colorpicker);
    }

}