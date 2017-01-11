using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UwpDragDrop.DragDrop
{
    /// <summary>
    /// Drag and drop gesture type
    /// </summary>
    public enum DragDropGestureType
    {
        /// <summary>
        /// The drag
        /// </summary>
        Drag,

        /// <summary>
        /// The drop
        /// </summary>
        Drop
    }

    /// <summary>
    /// Drag / Drop manager class
    /// </summary>
    public static class DragDropManager
    {
        private static Canvas _dragDropCanvas;
        public static Canvas DragDropCanvas
        {
            get
            {
                if (_dragDropCanvas == null)
                {
                    var frame = Window.Current.Content as MyFrame;
                    if (frame != null)
                    {
                        _dragDropCanvas = frame.DragDropCanvas;
                    }
                }

                return _dragDropCanvas;
            }
       }

        /// <summary>
        /// Occurs when [drag started for group].
        /// </summary>
        public static event EventHandler<string> DragStartedForGroup;

        /// <summary>
        /// Raises the drag started for group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        private static void RaiseDragStartedForGroup(string groupName)
        {
            var handler = DragStartedForGroup;
            if (handler != null)
            {
                handler(null, groupName);
            }
        }

        /// <summary>
        /// Occurs when [drag ended for group].
        /// </summary>
        public static event EventHandler<string> DragEndedForGroup;

        /// <summary>
        /// Raises the drag ended for group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        private static void RaiseDragEndedForGroup(string groupName)
        {
            var handler = DragEndedForGroup;
            if (handler != null)
            {
                handler(null, groupName);
            }
        }

        /// <summary>
        /// The groups of drag and drop panels
        /// </summary>
        private static readonly Dictionary<string, DragDropGroup> _Groups = new Dictionary<string, DragDropGroup>();

        /// <summary>
        /// Adds or upadtes the group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="placeholder">The panel.</param>
        /// <param name="type">The gesture type.</param>
        public static void AddOrUpdateGroup(string name, IDragDropPlaceholder placeholder, DragDropGestureType type)
        {
            var group = new DragDropGroup
            {
                DragPanels = new List<IDragPlaceholder>(),
                DropPanels = new List<IDropPlaceholder>()
            };

            var isGroupExist = false;

            // Get current group if already exists
            if (_Groups.ContainsKey(name))
            {
                group = _Groups[name];
                isGroupExist = true;
            }

            // Update panel value
            if (type == DragDropGestureType.Drag)
            {
                group.DragPanels.Add((IDragPlaceholder) placeholder);
            }
            else
            {
                group.DropPanels.Add((IDropPlaceholder) placeholder);
            }

            // Add or update group dictionary
            if (isGroupExist)
            {
                _Groups[name] = group;
            }
            else
            {
                _Groups.Add(name, group);
            }
        }

        /// <summary>
        /// Gets the drag placeholders.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The placeholder list if exists or <c>null</c>.
        /// </returns>
        public static List<IDragPlaceholder> GetDragPlaceholder(string name)
        {
            if (!string.IsNullOrEmpty(name) && _Groups.ContainsKey(name))
            {
                return _Groups[name].DragPanels;
            }

            return null;
        }

        /// <summary>
        /// Gets the drop placeholders.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The placeholder list if exists or <c>null</c>.
        /// </returns>
        public static List<IDropPlaceholder> GetDropPlaceholder(string name)
        {
            if (_Groups.ContainsKey(name))
            {
                return _Groups[name].DropPanels;
            }

            return null;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The data</returns>
        public static object GetData(string name)
        {
            if (_Groups.ContainsKey(name))
            {
                return _Groups[name].Data;
            }

            return null;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        public static void SetData(string name, object data)
        {
            if (_Groups.ContainsKey(name))
            {
                _Groups[name].Data = data;
            }
        }

        /// <summary>
        /// Gets the overlay.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The overlay</returns>
        public static FrameworkElement GetOverlay(string name)
        {
            if (_Groups.ContainsKey(name))
            {
                return _Groups[name].Overlay;
            }

            return null;
        }

        /// <summary>
        /// Sets the overlay.
        /// </summary>
        /// <param name="groupName">The name.</param>
        /// <param name="element">The element.</param>
        public static void SetOverlay(string groupName, FrameworkElement element)
        {
            if (_Groups.ContainsKey(groupName))
            {
                if (element == null)
                {
                    RaiseDragEndedForGroup(groupName);
                }
                else
                {
                    RaiseDragStartedForGroup(groupName);
                }

                _Groups[groupName].Overlay = element;
            }
        }

        /// <summary>
        /// Stops the drag.
        /// </summary>
        /// <returns>A task</returns>
        public static async Task StopDragAsync()
        {
            if (_Groups == null)
            {
                return;
            }

            //if (DispatcherHelper.UIDispatcher != null)
            //{
            //    await DispatcherHelper.UIDispatcher.RunIdleAsync(_ =>
            //    {
            //        // Clear drag overlays
            //        if (App.RootFrame.DragDropCanvas != null)
            //        {
            //            App.RootFrame.DragDropCanvas.Children.Clear();
            //        }
            //    });
            //}

            foreach (var dragDropGroup in _Groups)
            {
                if (dragDropGroup.Value == null || dragDropGroup.Value.DragPanels == null)
                {
                    continue;
                }

                foreach (var dragPanel in dragDropGroup.Value.DragPanels)
                {
                    if (dragPanel != null)
                    {
                        dragPanel.IsDragStarted = false;
                    }
                }
            }
        }
    }
}