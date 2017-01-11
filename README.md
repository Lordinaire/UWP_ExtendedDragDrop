UWP Extended Drag & Drop
===========

This project demonstrates how to create a drag & drop system. It doesn't *extend* the native UWP drag & drop system !

## Build status

## Getting started

### Custom frame 

In order to display item that you want to drag in top of all items, you need to create a custom frame like this one :

```
<Style TargetType="local:MyFrame">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="local:MyFrame">
                <Grid x:Name="PART_RootGrid">

                    <ContentPresenter x:Name="contentPresenter" />

                    <Canvas x:Name="PART_DragDropCanvas"
                            HorizontalAlignment="Stretch"
                            VerticalAlignment="Stretch" />
                </Grid>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

We just add a canvas named `PART_DragDropCanvas` on top of everything.

### Create groups

The system is based on group name to match wich content can be drag to a drop place. A 'drag' placeholder can have several 'drop' placeholders.
In your code, if you want to link two placeholders (one for dragging and one for dropping) you just need to write something like that :

```
<DragPlaceholder GroupName="Operations">
...
</DragPlaceholder>

<DropPlaceholder GroupName="Operations"
                 IsDropable="True">
...
</DropPlaceholder>
```

### Drop logic

Of course, you need to implement your own logic when the user drop a content. A simple way is to inherit of the `DropPlaceholder` class.

This sample demonstrates how to do that :

```
public class DebugDropPlaceholder : DropPlaceholder
{
    public override void DropContent(object content)
    {
        base.DropContent(content);

        Debug.WriteLine("DROP !");
    }
}
```

A lot of methods are overridable if you want specific behaviors like effects, animations...

### Showcase

![](http://blog.lordinaire.fr/wp-content/uploads/2017/01/ExtendedDragDropSample.gif)
