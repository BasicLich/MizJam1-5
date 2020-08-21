using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MizJam1.UIComponents
{
    public class UIImage : UIComponent
    {
        public UIImage(Texture2D image)
        {
            Image = image;
            ImageColor = Color.White;
            Size = image.Bounds.Size;
        }

        public UIImage(Texture2D image, Texture2D selectedImage) : this(image)
        {
            SelectedImage = selectedImage;
        }

        /// <summary>
        /// This element's image
        /// </summary>
        /// <value>The image.</value>
        public Texture2D Image { get; set; }
        public Texture2D SelectedImage { get; set; }
        /// <summary>
        /// This image's masking color. White shows the image as is.
        /// </summary>
        /// <value>The color of the image.</value>
        public Color ImageColor { get; set; }

        /// <summary>
        /// Draws itself and it's image.
        /// </summary>
        /// <param name="sb">The sprite batch to draw with.</param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (Selected && SelectedImage != null)
            {
                sb.Draw(SelectedImage, new Rectangle(AbsolutePosition, Size), Color.White);
            } else
            {
                sb.Draw(Image, new Rectangle(AbsolutePosition, Size), Color.White);
            }
        }
    }
}
