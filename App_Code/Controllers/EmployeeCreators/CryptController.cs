using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Broker.Controllers.EmployeeManagement {

    /// <summary>
    /// Klasa za kriptografija
    /// </summary>
    public class SymmCrypto {

        /// <summary>
        /// Mnozestvo so algorimti za kriptiranje
        /// </summary>
        public enum SymmProvEnum : int {
            DES, RC2, Rijndael
        }

        private SymmetricAlgorithm mobjCryptoService;


        /// <summary>
        /// Se instancira nov tip na algoritam za kriptografija 
        /// vo zavisnost od izbraniot tip na kriptografija
        /// </summary>
        /// <param name="NetSelected">SymmProvEnum NetSelected</param>
        public SymmCrypto(SymmProvEnum NetSelected) {
            switch (NetSelected) {
                case SymmProvEnum.DES:
                    mobjCryptoService = new DESCryptoServiceProvider();
                    break;
                case SymmProvEnum.RC2:
                    mobjCryptoService = new RC2CryptoServiceProvider();
                    break;
                case SymmProvEnum.Rijndael:
                    mobjCryptoService = new RijndaelManaged();
                    break;
            }
        }

        /// <summary>
        /// Preotovaren konstruktor na klasata za kriptografija
        /// </summary>
        /// <param name="ServiceProvider">SymmetricAlgorithm ServiceProvider</param>
        public SymmCrypto(SymmetricAlgorithm ServiceProvider) {
            mobjCryptoService = ServiceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key">string Key</param>
        /// <returns></returns>
        private byte[] GetLegalKey(string Key) {
            string sTemp = Key;
            if (mobjCryptoService.LegalKeySizes.Length > 0) {
                int moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;
                if (sTemp.Length * 8 > mobjCryptoService.LegalKeySizes[0].MaxSize)
                    // get the left of the key up to the max size allowed
                    sTemp = sTemp.Substring(0, mobjCryptoService.LegalKeySizes[0].MaxSize / 8);
                else if (sTemp.Length * 8 < moreSize)
                    if (mobjCryptoService.LegalKeySizes[0].SkipSize == 0)
                        // simply pad the key with spaces up to the min size allowed
                        sTemp = sTemp.PadRight(moreSize / 8, ' ');
                    else {
                        while (sTemp.Length * 8 > moreSize)
                            moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;

                        sTemp = sTemp.PadRight(moreSize / 8, ' ');
                    }
            }

            // convert the secret key to byte array
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        public string Encrypting(string Source, string Key) {
            byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(System.Web.HttpUtility.UrlEncode(Source));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            byte[] bytKey = GetLegalKey(Key);

            // set the private key
            mobjCryptoService.Key = bytKey;
            mobjCryptoService.IV = bytKey;

            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();

            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            byte[] bytOut = ms.GetBuffer();
            int i = 0;
            for (i = 0; i < bytOut.Length; i++)
                if (bytOut[i] == 0)
                    break;

            return System.Convert.ToBase64String(bytOut, 0, i);
        }

        public string Decrypting(string Source, string Key) {
            byte[] bytIn = System.Convert.FromBase64String(Source);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

            byte[] bytKey = GetLegalKey(Key);

            mobjCryptoService.Key = bytKey;
            mobjCryptoService.IV = bytKey;

            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            System.IO.StreamReader sr = new System.IO.StreamReader(cs);
            string sEncoded = sr.ReadToEnd();
            return System.Web.HttpUtility.UrlDecode(sEncoded);
        }

        public string base64Encode(string sData) {
            try {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            } catch (Exception ex) {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }


        public string base64Decode(string sData) {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(sData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}
