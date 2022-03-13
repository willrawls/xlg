using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class AddSet_ExpectedKeysTests
    {
        private const string SetText = @"
When CapsLock CapsLock G G type william.rawls@gmail.com
  Or W type william.rawls@smarsh.com
  Or H type willrawls@hotmail.com
  Or U type gustavo.jimenez9107@gmail.com
  Or X ## type x~~y
  Or Y **** type Fred:~~~~:\*\*:\n;\\;
  Or A type 2279 Perez St Apt 290
  Or N type William\tRawls
  Or Z run ""C:\Windows\notepad.exe"" ""~I:\OneDrive\Pictures\In\j498jddp6q961 - Copy.png~""
//  Or F type Attached please find my resume. This looks like a pretty good fit.
//  Or M type Attached please find my resume. This may be a good fit. Can you provide some more details, please?
//  Or C type You can call me any time. I'm getting a lot of unknown phone calls going to voicemail. So I will need the phone number you'll be calling from so I can add it to my contacts. Alternatively, I can call you.
//  Or R type Well it depends on if your company offers a good medical plan or not. If you offer no medical or your medical is not as good as what I have, I charge $55/hour W2. If you have a better medical plan that doesn't cost too much, I can go down to $50/hour W2.
CapsLock CapsLock P 3 type 850-320-1934
  Or 4 type 850-270-1381
When CapsLock CapsLock E O type eve online 
When CapsLock CapsLock T L type torchlight 2 
When CapsLock CapsLock C choose
  abc
  def
  ghi{BACKSPACE}{ENTER}{TAB}jkl
When CapsLock CapsLock Q 1 type mno{BACKSPACE}p{ENTER}q{speed 50}{TAB}r{pause 500} tu{clipboard}{speed 1}v{guid b}w{roll 3 6}xyz
Include 
When CapsLock CapsLock N W 1 type Level 50 Healer LFG. DM me
When CapsLock CapsLock S N W type new world 
When CapsLock CapsLock 2 2 7 9 type 2279 Perez St Apt 290		Salinas	CA	93906
When CapsLock CapsLock M 1 move to 100 100 700 500
When CapsLock CapsLock M 2 move percent 10 10 25 25
When CapsLock CapsLock R 1 random 1 d 100
";

        [TestMethod]
        public void AddSet_SetText_Basic()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(SetText);

            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.G }, actual[0].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.W }, actual[1].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.H }, actual[2].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.U }, actual[3].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.X }, actual[4].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.Y }, actual[5].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.A }, actual[6].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.N }, actual[7].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.G, PKey.Z }, actual[8].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.P, PKey.D3 }, actual[9].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.P, PKey.D4 }, actual[10].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.E, PKey.O }, actual[11].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.T, PKey.L }, actual[12].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.C }, actual[13].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.Q, PKey.D1 }, actual[14].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.N, PKey.W, PKey.D1 }, actual[15].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.S, PKey.N, PKey.W }, actual[16].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.D2, PKey.D2, PKey.D7, PKey.D9 }, actual[17].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.M, PKey.D1 }, actual[18].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.M, PKey.D2 }, actual[19].Sequence);
            My.AssertAllAreEqual( new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.R, PKey.D1 }, actual[20].Sequence);
        }

    }
}