#region Microsoft Community License
/*****
Microsoft Community License (Ms-CL)
Published: October 12, 2006

   This license governs  use of the  accompanying software. If you use
   the  software, you accept this  license. If you  do  not accept the
   license, do not use the software.

1. Definitions

   The terms "reproduce,"    "reproduction," "derivative works,"   and
   "distribution" have  the same meaning here  as under U.S. copyright
   law.

   A  "contribution" is the  original  software, or  any additions  or
   changes to the software.

   A "contributor"  is any  person  that distributes  its contribution
   under this license.

   "Licensed  patents" are  a contributor's  patent  claims that  read
   directly on its contribution.

2. Grant of Rights

   (A) Copyright   Grant-  Subject to  the   terms  of  this  license,
   including the license conditions and limitations in section 3, each
   contributor grants   you a  non-exclusive,  worldwide, royalty-free
   copyright license to reproduce its contribution, prepare derivative
   works of its  contribution, and distribute  its contribution or any
   derivative works that you create.

   (B) Patent Grant-  Subject to the terms  of this license, including
   the   license   conditions and   limitations   in  section  3, each
   contributor grants you   a non-exclusive, worldwide,   royalty-free
   license under its licensed  patents to make,  have made, use, sell,
   offer   for   sale,  import,  and/or   otherwise   dispose  of  its
   contribution   in  the  software   or   derivative  works  of   the
   contribution in the software.

3. Conditions and Limitations

   (A) Reciprocal  Grants- For any  file you distribute  that contains
   code from the software (in source code  or binary format), you must
   provide recipients the source code  to that file  along with a copy
   of this  license,  which license  will  govern that  file.  You may
   license other  files that are  entirely  your own  work and  do not
   contain code from the software under any terms you choose.

   (B) No Trademark License- This license does not grant you rights to
   use any contributors' name, logo, or trademarks.

   (C)  If you  bring  a patent claim    against any contributor  over
   patents that you claim  are infringed by  the software, your patent
   license from such contributor to the software ends automatically.

   (D) If you distribute any portion of the  software, you must retain
   all copyright, patent, trademark,  and attribution notices that are
   present in the software.

   (E) If  you distribute any  portion of the  software in source code
   form, you may do so only under this license by including a complete
   copy of this license with your  distribution. If you distribute any
   portion  of the software in  compiled or object  code form, you may
   only do so under a license that complies with this license.

   (F) The  software is licensed  "as-is." You bear  the risk of using
   it.  The contributors  give no  express  warranties, guarantees  or
   conditions. You   may have additional  consumer  rights  under your
   local laws   which  this license  cannot   change. To   the  extent
   permitted under  your local  laws,   the contributors  exclude  the
   implied warranties of   merchantability, fitness for  a  particular
   purpose and non-infringement.


*****/
#endregion

using System;
using System.Collections.Generic;
using System.Management.Automation.Provider;
using System.Xml;
using Microsoft.Office.OneNote.PowerShell.Provider;

namespace Microsoft.Office.OneNote.PowerShell.Provider
{
    class ContentReader: IContentReader
    {
        /// <summary>
        /// This is the internal XML representation of the page in question.
        /// </summary>
        private string [] _text;

        /// <summary>
        /// the offset we're reading into.
        /// </summary>
        private long _offset = 0;

        /// <summary>
        /// Creates a OneNote content reader for a particular onenote instantiation and item in the hierarchy.
        /// </summary>
        /// <param name="drive">Can be used to get to the OneNote instantiation.</param>
        /// <param name="node">The OneNote XML node that identifies the node to "get."</param>
        public ContentReader(OneNoteDriveInfo drive, XmlNode node)
        {
            string id = node.Attributes["ID"].Value;
            string xml;
            Microsoft.Office.Interop.OneNote.ApplicationClass _app = new Microsoft.Office.Interop.OneNote.ApplicationClass( );
            _app.GetPageContent(id, out xml, Microsoft.Office.Interop.OneNote.PageInfo.piBasic);
            XmlDocument doc = new XmlDocument( );
            doc.LoadXml(xml);
            _text = Microsoft.Office.OneNote.PowerShell.Utilities.PrettyPrintXml(doc).Split(new char[] { '\n' });
        }

        #region IContentReader Members

        public void Close( )
        {
            //
            //  NOTHING
            //
        }

        public System.Collections.IList Read(long readCount)
        {
            List<string> list = new List<string>( );
            if (readCount <= 0)
            {
                readCount = _text.Length;
            }
            while ((_offset < _text.Length) && (readCount > 0))
            {
                list.Add(_text[_offset].TrimEnd(new char[] { '\r', '\n' }));
                _offset++;
                readCount--;
            }
            return list;
        }

        public void Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose( )
        {
            //
            //  NOTHING
            //
        }

        #endregion
    }
}
