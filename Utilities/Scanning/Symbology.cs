namespace MonoCross.Utilities.Barcode
{
    // enumaration of 1 Dimensional barcode symbologies
    public enum Symbology
    {
        UNKNOWN,            // for scanners that don't report the scanned barcode output type
        UPCA,               // redlaser?, linea-pro, koamtec 300i
        UPCE,               // redlaser, linea-pro, koamtec 300i
        EAN8,               // redlaser, linea-pro, koamtec 300i
        EAN13,              // redlaser, linea-pro, koamtec 300i
        EAN128,              // redlaser, linea-pro, koamtec 300i
        Code11,             // koamtec 300i
        Code32,             // koamtec 300i
        Code39,             // redlaser, linea-pro, koamtec 300i
        Code93,             // redlaser (android only)
        Code128,            // redlaser, linea-pro, koamtec 300i
        Codabar,            // linea-pro, koamtec 300i
        I2of5,              // linea-pro, koamtec 300i
        Sticky,             // redlaser (no idea what this is but redlaser supports it)
        // others supported by koamtec 300i
        // GS1-128 (UCC/EAN128), MSI, Plessey, PosiCode, GS1 DataBar (Omni/Limited/Expanded), S2of5IA, S2of5ID, TLC39, Telepen, Trioptic
        DataMatrix,         // EMDK 2D barcode
        PDF417,             // EMDK 2D barcode
        QRCode,             // EMDK 2D barcode
    }
}
