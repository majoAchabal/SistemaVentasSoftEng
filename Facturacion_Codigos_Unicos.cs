using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using QRCoder;
using System.Drawing;

public class Facturacion
{
    // Conjuntos estáticos para almacenar códigos generados y evitar repeticiones
    private static HashSet<string> generatedCUIS = new HashSet<string>();
    private static HashSet<string> generatedCUFD = new HashSet<string>();
    private static HashSet<string> generatedCUF = new HashSet<string>();

    private string cuis;
    private string cufd;
    private string cuf;
    private DateTime cuisExpiry;
    private DateTime cufdExpiry;

    public string CUIS
    {
        get
        {
            if (DateTime.UtcNow > cuisExpiry || string.IsNullOrEmpty(cuis))
                GenerateCUIS();
            return cuis;
        }
    }

    public string CUFD
    {
        get
        {
            if (DateTime.UtcNow > cufdExpiry || string.IsNullOrEmpty(cufd))
                GenerateCUFD();
            return cufd;
        }
    }

    public string CUF
    {
        get { return cuf; }
        private set
        {
            if (value.Length != 34)
                throw new ArgumentException("CUF debe tener 34 caracteres.");
            cuf = value;
        }
    }

    public Facturacion()
    {
        GenerateCUIS();
        GenerateCUFD();
    }

    private void GenerateCUIS()
    {
        cuis = GenerateUniqueCode(16, generatedCUIS);
        cuisExpiry = DateTime.UtcNow.AddDays(365);
    }

    private void GenerateCUFD()
    {
        cufd = GenerateUniqueCode(64, generatedCUFD);
        cufdExpiry = DateTime.UtcNow.AddHours(24);
    }

    public void GenerateCUF()
    {
        cuf = GenerateUniqueCode(34, generatedCUF);
    }

    private string GenerateUniqueCode(int length, HashSet<string> existingCodes)
    {
        string code;
        do
        {
            code = GenerateRandomString(length);
        }
        while (!existingCodes.Add(code)); // Añade el código al HashSet y verifica si es único
        return code;
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        byte[] data = new byte[length];
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }
        var result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            result.Append(chars[data[i] % chars.Length]);
        }
        return result.ToString();
    }

    public Bitmap GenerateQRCode()
    {
        // Asegurar que CUF esté generado
        if (string.IsNullOrEmpty(this.CUF))
        {
            GenerateCUF();
        }

        // Preparar los datos para codificar en el código QR
        var qrContent = $"CUIS:{this.CUIS};CUFD:{this.CUFD};CUF:{this.CUF};FechaEmision:{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}";

        // Generar el código QR
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }
    }

    public void SaveQRCode(string filePath)
    {
        using (var qrCodeImage = GenerateQRCode())
        {
            qrCodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
