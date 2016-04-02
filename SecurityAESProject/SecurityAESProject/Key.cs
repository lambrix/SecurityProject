using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SecurityAESProject
{
    public class Key
    {
        //aes sleutel
        private byte[] aesKey;

        public byte[] AesKey
        {
            get { return aesKey; }
            set { aesKey = value; }
        }

        //RSA sleutels private & public
        private string publicKey;

        public string PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }

        private string privateKey;

        public string PrivateKey
        {
            get { return privateKey; }
            set { privateKey = value; }
        }




        //genereren van sleutel voor symmetrische encryptie
        public void GenerateAESKey()
        {
            try
            {
                using (AesManaged aes = new AesManaged())
                {
                    aes.GenerateKey();
                    //aes.GenerateIV();
                    aesKey = aes.Key;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GenerateRSAKey()
        {
            try
            {
                //Generate a public/private key pair.
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //Save the public key information to an RSAParameters structure.
                RSAParameters RSAKeyInfo = rsa.ExportParameters(false);

                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
