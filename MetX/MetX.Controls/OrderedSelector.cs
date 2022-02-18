using MetX.Standard.Library.Extensions;

namespace MetX.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public partial class OrderedSelector : UserControl
    {
        public OrderedSelector()
        {
            InitializeComponent();
        }

        public object[] CurrentSelection
        {
            get
            {
                if (SelectionList.Items.Count == 0)
                {
                    return Array.Empty<object>();
                }

                var ret = GetSelectedObjects(SelectionList);
                return ret;
            }
        }

        public ListBox.SelectedIndexCollection SelectedIndices =>
            // ReSharper disable once ConvertPropertyToExpressionBody
            SelectionList.SelectedIndices;

        public SelectionMode SelectionMode
        {
            get
            {
                return SelectionList.SelectionMode;
            }

            set
            {
                SelectionList.SelectionMode = value;
            }
        }

        public void Initialize(IEnumerable<string> sourceList, IEnumerable<string> selectionList)
        {
            SourceList.Items.Clear();
            SelectionList.Items.Clear();

            foreach (var item in sourceList)
            {
                SourceList.Items.Add(item);
            }

            foreach (var item in selectionList)
            {
                SelectionList.Items.Add(item);
            }
        }

        public void Initialize(object[] sourceList, object[] selectionList)
        {
            SourceList.Items.Clear();
            SelectionList.Items.Clear();

            SourceList.Items.AddRange(sourceList);
            SelectionList.Items.AddRange(selectionList);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (SourceList.SelectedItems.Count == 0)
                return;

            var objects = GetSelectedObjects(SourceList);
            AddItems(SelectionList, objects);
            RemoveSelectedItems(SourceList);
        }

        private void AddItems(ListBox list, object[] objects)
        {
            foreach (var item in objects)
            {
                var index = list.Items.Add(item);
                list.SetSelected(index, true);
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            var objectsSelected = GetSelectedObjects(SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            var indices = GetSortedSelectedIndicies(SelectionList);
            for (var i = indices.Length; i >= 0; i--)
            {
                if (indices[i] >= SelectionList.Items.Count)
                    continue;
                SelectionList.Items.RemoveAt(indices[i]);
                SelectionList.Items.Insert(indices[i] + 1, objectsSelected[i]);
            }
        }

        private int[] GetSelectedIndicies(ListBox list)
        {
            if (list.SelectedIndices.Count == 0)
            {
                return null;
            }

            var indices = new int[list.SelectedIndices.Count];
            list.SelectedIndices.CopyTo(indices, 0);
            return indices;
        }

        private object[] GetSelectedObjects(ListBox list)
        {
            if (list.SelectedIndices.Count == 0)
            {
                return null;
            }

            var objects = new object[list.SelectedIndices.Count];
            for (var index = 0; index < list.SelectedItems.Count; index++)
            {
                objects[index] = list.SelectedItems[index];
            }

            return objects;
        }

        private int[] GetSortedSelectedIndicies(ListBox list)
        {
            if (list.SelectedIndices.Count == 0)
            {
                return null;
            }

            var indices = GetSelectedIndicies(list);
            Array.Sort(indices);
            return indices;
        }

        private void LastButton_Click(object sender, EventArgs e)
        {
            var objectsSelected = GetSelectedObjects(SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            var indices = GetSortedSelectedIndicies(SelectionList);
            for (var i = indices.Length; i >= 0; i--)
            {
                SelectionList.Items.RemoveAt(indices[i]);
            }

            foreach (var item in objectsSelected)
            {
                SelectionList.Items.Add(item);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (SelectionList.SelectedIndices.Count == 0)
            {
                return;
            }

            var objects = GetSelectedObjects(SelectionList);
            RemoveSelectedItems(SelectionList);
            AddItems(SourceList, objects);
        }

        private void RemoveSelectedItems(ListBox list)
        {
            if (SelectionList.SelectedIndices.Count == 0)
            {
                return;
            }

            var indices = GetSortedSelectedIndicies(SelectionList);
            for (var index = indices.Length - 1; index >= 0; index--)
            {
                list.Items.RemoveAt(indices[index]);
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            var objectsSelected = GetSelectedObjects(SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            var indices = GetSortedSelectedIndicies(SelectionList);
            for (var i = indices.Length; i >= 0; i--)
            {
                if (indices[i] == 0)
                    continue;
                SelectionList.Items.RemoveAt(indices[i]);
                var newIndex = indices[i] - 1;
                SelectionList.Items.Insert(newIndex, objectsSelected[i]);
                SelectionList.SetSelected(newIndex, true);
            }
        }
    }
}
