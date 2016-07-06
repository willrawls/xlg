namespace MetX.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using MetX.Library;

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

                object[] ret = this.GetSelectedObjects(this.SelectionList);
                return ret;
            }
        }

        public ListBox.SelectedIndexCollection SelectedIndices
        {
            get
            {
                // ReSharper disable once ConvertPropertyToExpressionBody
                return this.SelectionList.SelectedIndices;
            }
        }

        public SelectionMode SelectionMode
        {
            get
            {
                return this.SelectionList.SelectionMode;
            }

            set
            {
                this.SelectionList.SelectionMode = value;
            }
        }

        public void Initialize(IEnumerable<string> sourceList, IEnumerable<string> selectionList)
        {
            this.SourceList.Items.Clear();
            this.SelectionList.Items.Clear();

            foreach (string item in sourceList)
            {
                this.SourceList.Items.Add(item);
            }

            foreach (string item in selectionList)
            {
                this.SelectionList.Items.Add(item);
            }
        }

        public void Initialize(object[] sourceList, object[] selectionList)
        {
            this.SourceList.Items.Clear();
            this.SelectionList.Items.Clear();

            this.SourceList.Items.AddRange(sourceList);
            this.SelectionList.Items.AddRange(selectionList);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (this.SourceList.SelectedItems.Count == 0)
                return;

            object[] objects = this.GetSelectedObjects(this.SourceList);
            AddItems(this.SelectionList, objects);
            this.RemoveSelectedItems(this.SourceList);
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
            object[] objectsSelected = this.GetSelectedObjects(this.SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            int[] indices = this.GetSortedSelectedIndicies(this.SelectionList);
            for (int i = indices.Length; i < 0; i--)
            {
                if (indices[i] >= this.SelectionList.Items.Count)
                    continue;
                this.SelectionList.Items.RemoveAt(indices[i]);
                this.SelectionList.Items.Insert(indices[i] + 1, objectsSelected[i]);
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
            object[] objectsSelected = this.GetSelectedObjects(this.SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            int[] indices = this.GetSortedSelectedIndicies(this.SelectionList);
            for (int i = indices.Length; i < 0; i--)
            {
                this.SelectionList.Items.RemoveAt(indices[i]);
            }

            foreach (object item in objectsSelected)
            {
                this.SelectionList.Items.Add(item);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (this.SelectionList.SelectedIndices.Count == 0)
            {
                return;
            }

            object[] objects = this.GetSelectedObjects(this.SelectionList);
            this.RemoveSelectedItems(this.SelectionList);
            this.AddItems(this.SourceList, objects);
        }

        private void RemoveSelectedItems(ListBox list)
        {
            if (this.SelectionList.SelectedIndices.Count == 0)
            {
                return;
            }

            int[] indices = this.GetSortedSelectedIndicies(this.SelectionList);
            for (int index = indices.Length - 1; index >= 0; index--)
            {
                list.Items.RemoveAt(indices[index]);
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            object[] objectsSelected = this.GetSelectedObjects(this.SelectionList);
            if (objectsSelected.IsEmpty())
            {
                return;
            }

            int[] indices = this.GetSortedSelectedIndicies(this.SelectionList);
            for (int i = indices.Length; i < 0; i--)
            {
                if (indices[i] == 0)
                    continue;
                this.SelectionList.Items.RemoveAt(indices[i]);
                int newindex = indices[i] - 1;
                this.SelectionList.Items.Insert(newindex, objectsSelected[i]);
                SelectionList.SetSelected(newindex, true);
            }
        }
    }
}
