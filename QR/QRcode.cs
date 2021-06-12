using System.Drawing;
using ZXing;

namespace QR
{
    public class QRcode
    {
        /// <summary>
        /// 识别图中二维码
        /// </summary>
        /// <param name="barcodeBitmap">图片</param>
        /// <returns>内容</returns>
        public static string DecodeQrCode(Bitmap barcodeBitmap)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var result = reader.Decode(barcodeBitmap);
            return (result == null) ? null : result.Text;
        }

         /// <summary>
         /// 生成二维码
         /// </summary>
         /// <param name="msg">内容</param>
         /// <param name="codeSizeInPixels">长宽</param>
         /// <param name="Margin">边框（暂未使用，填0即可）</param>
         /// <returns>图片</returns>
         public static Bitmap BulidQRcode(string msg,int codeSizeInPixels,int Margin)
         {
             BarcodeWriter writer = new BarcodeWriter();
             writer.Format = BarcodeFormat.QR_CODE;
             writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");//编码问题
             writer.Options.Hints.Add(
                 EncodeHintType.ERROR_CORRECTION,
                 ZXing.QrCode.Internal.ErrorCorrectionLevel.H
 
             );
             writer.Options.Height = writer.Options.Width = codeSizeInPixels;
             ZXing.Common.BitMatrix bm = writer.Encode(msg);
             Bitmap img = writer.Write(bm);
             return img;
         }
}
}
