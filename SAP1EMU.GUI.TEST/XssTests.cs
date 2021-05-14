using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.GUI.Test
{
    public class XssTests
    {
        // TODO: Write tests to check for XSS in POST, and URL?GET

        // LDA 0x0
        // <script>alert('xss')</script>
        // ...
        // HLT 0x0

        // https://localhost:5001/Emulator/SAP1h=%3Cscript%3Ealert('xss')%3C/script%3E
        // https://localhost:5001/Emulator/SAP1?h=%3Cscript%3Ealert('xss')%3C/script%3E
    }
}
