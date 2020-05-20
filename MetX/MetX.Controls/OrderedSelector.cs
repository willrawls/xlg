namespace MetX.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Library;

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
                    return new object[0];
                }

                object[] ret = GetSelectedObjects(SelectionList);
                return ret;
            }
        }

        public ListBox.SelectedIndexCollection SelectedIndices
        {
            get
            {
                // ReSharper disable once ConvertPropertyToExpressionBody
                return SelectionList.SelectedIndices;
            }
        }

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

            foreach (string item in sourceList)
            {
                SourceList.Items.Add(item);
            }

            foreach (string item in selectionList)
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

            object[] objects = GetSelectedObjects(SourceList);
            AddItems(SelectionList, objects);
            RemoveSelectedItems(SourceList);
        }

        private void AddItems(ListBox list, object[] objects)
        {
            foreach (object item in objects)
            {
                int index = list.Items.Add(item);
                list.SetSelected(index, true);
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            object[] objectsSelected = GetSelectedObjects(SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            int[] indices = GetSortedSelectedIndicies(SelectionList);
            for (int i = indices.Length; i < 0; i--)
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

            int[] indices = new int[list.SelectedIndices.Count];
            list.SelectedIndices.CopyTo(indices, 0);
            return indices;
        }

        private object[] GetSelectedObjects(ListBox list)
        {
            if (list.SelectedIndices.Count == 0)
            {
                return null;
            }

            object[] objects = new object[list.SelectedIndices.Count];
            for (int index = 0; index < list.SelectedItems.Count; index++)
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

            int[] indices = GetSelectedIndicies(list);
            Array.Sort(indices);
            return indices;
        }

        private void LastButton_Click(object sender, EventArgs e)
        {
            object[] objectsSelected = GetSelectedObjects(SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            int[] indices = GetSortedSelectedIndicies(SelectionList);
            for (int i = indices.Length; i < 0; i--)
            {
                SelectionList.Items.RemoveAt(indices[i]);
            }

            foreach (object item in objectsSelected)
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

            object[] objects = GetSelectedObjects(SelectionList);
            RemoveSelectedItems(SelectionList);
            AddItems(SourceList, objects);
        }

        private void RemoveSelectedItems(ListBox list)
        {
            if (SelectionList.SelectedIndices.Count == 0)
            {
                return;
            }

            int[] indices = GetSortedSelectedIndicies(SelectionList);
            for (int index = indices.Length - 1; index >= 0; index--)
            {
                list.Items.RemoveAt(indices[index]);
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            object[] objectsSelected = GetSelectedObjects(SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            int[] indices = GetSortedSelectedIndicies(SelectionList);
            for (int i = indices.Length; i < 0; i--)
            {
                if (indices[i] == 0)
                    continue;
                SelectionList.Items.RemoveAt(indices[i]);
                int newindex = indices[i] - 1;
                SelectionList.Items.Insert(newindex, objectsSelected[i]);
                SelectionList.SetSelected(newindex, true);
            }
        }
    }
}
