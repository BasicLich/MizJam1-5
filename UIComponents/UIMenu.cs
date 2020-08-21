using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MizJam1.UIComponents
{
    public class UIMenu : UIContainer
    {
        public UIMenu(Point position, Point size, bool keepCentered = true) : base(position, size, keepCentered) { }

        /// <summary>
        /// Adds the element to this Menu.
        /// </summary>
        /// <param name="child">Child.</param>
        public override void AddChild(UIComponent child)
        {
            child.SetScale(Scale);
            children.Add(child);
            child.SetParent(this);

            if (KeepCentered) CenterPadding();
            else
            {
                UpdateChildrenAlignment();
            }
        }

        public override void SetScale(int scale)
        {
            base.SetScale(scale);

            foreach (UIComponent child in children)
            {
                child.SetScale(scale);
            }

            if (KeepCentered) CenterPadding();
            UpdateChildrenAlignment();
        }

        /// <summary>
        /// Sets the padding for this menu's elements, and centers them between each other
        /// </summary>
        /// <param name="padding">Padding.</param>
        public override void SetPadding(Point padding)
        {
            PaddingUpLeft = padding;

            UIComponent first = children.FirstOrDefault();

            if (first == null) return;

            first.Position = padding;

            UpdateChildrenAlignment();
        }

        public override Point PaddingDownRight => Size
            - PaddingUpLeft
            - (children.LastOrDefault()?.Size ?? Point.Zero);

        /// <summary>
        /// The amount of space between the elements of this menu.
        /// </summary>
        /// <value>The space between children.</value>
        public virtual int SpaceBetweenChildren { get => spaceBetweenChildren; set => spaceBetweenChildren = value; }

        /// <summary>
        /// Whether or not this element's children are aligned vertically.
        /// </summary>
        /// <value><c>true</c> if vertical; otherwise, <c>false</c>.</value>
        public virtual bool Vertical { get; set; }

        /// <summary>
        /// Whether or not this element's children are aligned horizontally.
        /// </summary>
        /// <value><c>true</c> if horizontal; otherwise, <c>false</c>.</value>
        public virtual bool Horizontal { get { return !Vertical; } set { Vertical = !value; } }

        private List<UIComponent> children = new List<UIComponent>();
        public IReadOnlyList<UIComponent> Children => children.AsReadOnly();
        private int spaceBetweenChildren;

        /// <summary>
        /// Centers the children of this menu.
        /// </summary>
        public override void CenterPadding()
        {
            if (!children.Any()) return;

            UIComponent first = children.First();

            if (Vertical)
            {
                first.Position = new Point((Width - first.Width) / 2, (Height - TotalChildrenHeight) / 2);
            }
            else
            {
                first.Position = new Point((Width - TotalChildrenWidth) / 2, (Height - first.Height) / 2);
            }

            UpdateChildrenAlignment();
        }

        private int TotalChildrenHeight => children.Sum(c => c.Height + SpaceBetweenChildren) - SpaceBetweenChildren;
        private int TotalChildrenWidth => children.Sum(c => c.Width + SpaceBetweenChildren) - SpaceBetweenChildren;

        /// <summary>
        /// Updates the children alignment.
        /// </summary>
        private void UpdateChildrenAlignment()
        {
            if (!children.Any()) return;

            UIComponent previous = children.First();
            previous.Position = PaddingUpLeft;

            Point newSize = previous.Size;

            if (Vertical)
            {
                foreach (UIComponent child in children.Skip(1))
                {
                    child.Position = new Point(
                      previous.X + (previous.Width - child.Width),
                      previous.Y + previous.Height + SpaceBetweenChildren);
                    newSize = new Point(Math.Max(newSize.X, child.X), newSize.Y + child.Height + SpaceBetweenChildren);
                    previous = child;
                }
            }
            else
            {
                foreach (UIComponent child in children.Skip(1))
                {
                    child.Position = new Point(
                      previous.X + previous.Width + SpaceBetweenChildren,
                      previous.Y + (previous.Height - child.Height));
                    newSize = new Point(newSize.X + child.Width + SpaceBetweenChildren, Math.Max(newSize.Y, child.Y));

                    previous = child;
                }
            }

            newSize += PaddingUpLeft;
            Size = newSize;
        }

        /// <summary>
        /// Simply sets this menu as selected
        /// </summary>
        public override void Select() => Selected = true;
        /// <summary>
        /// Simply sets this menu as deselected
        /// </summary>
        public override void Deselect() => Selected = false;

        /// <summary>
        /// Draws this component, then all of it's children.
        /// </summary>
        /// <param name="sb">Sb.</param>
        public override void Draw(SpriteBatch sb)
        {
            if (CurrentBackgroundImage != null)
            {
                sb.Draw(CurrentBackgroundImage, new Rectangle(AbsolutePosition, Size), CurrentBackgroundColor);
            }

            foreach (UIComponent child in children)
            {
                child.Draw(sb);
            }
        }

        /// <summary>
        /// Executes this element's command, and then the command of it's selected children.
        /// </summary>
        public override void Execute()
        {
            base.Execute();
            foreach (UIComponent child in children)
            {
                if (child.Selected)
                {
                    child.Execute();
                }
            }
        }

        private Point size;
        public override Point Size { get => size; set => size = value; }
    }
}
