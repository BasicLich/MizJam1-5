using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MizJam1.UIComponents
{
    public class UILabel : UIComponent
    {
        public UILabel(string text, SpriteFont font, Color textColor)
        {
            Text = text;
            Font = font;
            TextColor = textColor;
        }

        /// <summary>
        /// This label's text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        /// <summary>
        /// This label's font.
        /// </summary>
        /// <value>The font.</value>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// The text color of this label
        /// </summary>
        /// <value>The text color of this label.</value>
        public Color TextColor { get; set; }
        public Color? SelectedTextColor { get; set; }
        public Color CurrentTextColor => Selected ? SelectedTextColor ?? TextColor : TextColor;

        /// <summary>
        /// Returns the size of the label
        /// </summary>
        /// <value>The size.</value>
        public override Point Size => (Font.MeasureString(Text) * Scale).ToPoint();

        /// <summary>
        /// Draws itself and it's text.
        /// </summary>
        /// <param name="sb">The spritebatch to use to draw.</param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.DrawString(Font,
                          Text, 
                          AbsolutePosition.ToVector2(), 
                          CurrentTextColor, 
                          0, 
                          Vector2.Zero, 
                          Scale, 
                          SpriteEffects.None, 
                          0);
        }
    }
}
