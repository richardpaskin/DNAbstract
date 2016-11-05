using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DNAbstract.DNAbstract;

namespace DNAbstract
{
    class fileHandler
    {
        public Boolean fH_openAndCheckHeader(string DNASeqFilename)
        {
            String fileLine;

            Boolean headerResult = false;

            System.IO.StreamReader DNASeqFile = new System.IO.StreamReader(DNASeqFilename);

            fileLine = DNASeqFile.ReadLine();

            Console.WriteLine("first line:" + fileLine);

            if (fileLine.Substring(0, 1) == ">")
            {
                headerResult = true;

                Console.WriteLine("extract database name from definition line");
            }
            else
            {
                headerResult = false;
            }

            return (headerResult);
        }


        public int fH_CountDefinitionLines(string DNASeqFilename)
        {
            //                                      // ============================================
            //                                      // Count the definition lines in the whole file
            //                                      // paramters: The DNA sequnce file name
            //                                      // returns  - success: live count
            //                                      //          - failure: 0
            //                                      // ============================================
            String fileLine;

            int definitionLineCount = 0;

            System.IO.StreamReader DNASeqFile = new System.IO.StreamReader(DNASeqFilename);

            while ((fileLine = DNASeqFile.ReadLine()) != null)
            {
                if (fileLine.Substring(0, 1) == ">")
                {
                    definitionLineCount = definitionLineCount + 1;
                }
                
            };

            Console.WriteLine("definitionlines" + definitionLineCount.ToString());

            return (definitionLineCount);
        }

        public ParseDefnRslt fH_ParseDefnLine(string lineToParse) 
        {
            //                                      // 
            DNAbstract.ParseDefnRslt parseResults = new DNAbstract.ParseDefnRslt();
            //                                      // posit 
            parseResults.ParsedDefnStat = true;
            //                                      // check the definiton line has the identifying 
            if (lineToParse.Substring(0, 1) != ">")
            {
                parseResults.ParsedDefnDtls = "";
                parseResults.ParseDefnMsg = "Definition line expected, but got (10): "+ lineToParse.Substring(0, 1);
                parseResults.ParsedDefnStat = false;
                return (parseResults);
            }

            string defnLineGeneIdStrStrt = "[gene=";
            string defnLineGeneIdStrEnd = "]";
            int defnLineGeneIdStrLen = defnLineGeneIdStrStrt.Length;

            string defnLineAfterGeneIdStr;

            int defnLineGeneIdStrPosn = lineToParse.IndexOf(defnLineGeneIdStrStrt);
            int defnLineGeneIdEndPosn;

            //                                      // ? gene name identifying text found 
            if (defnLineGeneIdStrPosn > 0)
            {
            //                                      //        
                defnLineAfterGeneIdStr = lineToParse.Substring(defnLineGeneIdStrPosn);
                defnLineGeneIdEndPosn = defnLineAfterGeneIdStr.IndexOf(defnLineGeneIdStrEnd);
            }
            else
            {
                parseResults.ParsedDefnDtls = "";
                parseResults.ParseDefnMsg = "Could not find gene name id text";
                parseResults.ParsedDefnStat = false;
                return (parseResults);
            }

            //                                      // ? 
            if (defnLineGeneIdEndPosn > 0)
            {
                parseResults.ParsedDefnDtls = lineToParse.Substring(defnLineGeneIdStrPosn + defnLineGeneIdStrLen, defnLineGeneIdEndPosn - defnLineGeneIdStrLen);
                parseResults.ParseDefnMsg = "OK";
            }
            else
            {
                parseResults.ParsedDefnDtls = "";
                parseResults.ParseDefnMsg = "Could not find gene name end text";
                parseResults.ParsedDefnStat = false;
                return (parseResults);
            }

            return (parseResults);
        }

        public Boolean fH_ParseSeqBlock(string CurrentLine, 
                                        System.IO.StreamReader streamReaderReaderIn,  
                                        out string seqBlockParsedStr, 
                                        out string seqBlockParseMsg)
        {
            //                                      // ================================================================
            //                                      // method to parse a line of DNA sequence records 
            //                                      // recieved: the current line and the stramreader 
            //                                      // returns  - success: single compund string of DNA sequence codons 
            //                                      //          - failure: reason message
            //                                      // ================================================================

            //                                      // -------------------------
            //                                      // check that the first three characers (a codon) of the first line in the block 
            //                                      // contain one of the four gene letters;
            //                                      // -------------------------              

            if (checkSeqBlckStrt(CurrentLine) == true)
            {
                //                                      // DNA sequence rec starts with a codon - OK (nothing more to do than return it)
                seqBlockParsedStr = CurrentLine;
                seqBlockParseMsg = "OK";
                return (true);
            }
            else
            {
                //                                      // pass back fail result
                seqBlockParsedStr = "";
                seqBlockParseMsg = "leading portion of sequence data not a codon";
                return(false);
            }


            //                                      //  end parse seq block
        }
        
        public bool checkSeqBlckStrt(string p_SeqBlockLine)
        //                                      // ============================================================
        //                                      // function to check for a codon at the start of a DNA seq line
        //                                      // ============================================================
        {
            //                                      // posit bad deq data
            bool checkSeqBlckStrtrslt = false;
            //                                      // var to store the set of nucleotide chracters 
            string neucleotides = "ACGT";
            //                                      // extract first codon character
            string stringLeadPartChr1 = p_SeqBlockLine.Substring(0, 1);
            //string stringLeadPartChr2 = p_SeqBlockLine.Substring(1, 1);
            //string stringLeadPartChr3 = p_SeqBlockLine.Substring(2, 1);
            //                                      // ? first character is a valid nucleodite character
            if (
                    (neucleotides.IndexOf(stringLeadPartChr1) > -1)
                 //|| (neucleotides.IndexOf(stringLeadPartChr2) > -1)
                 //|| (neucleotides.IndexOf(stringLeadPartChr3) > -1)
                )
            {
                //                                      // DNA sequence rec starts with a codon - OK
                checkSeqBlckStrtrslt = true;
            }
            else
            {
                checkSeqBlckStrtrslt = false;


            }
            return (checkSeqBlckStrtrslt);
        //                                      // end chckSeqBlckStrt class
        }

        public static void WriteSeqLnesToFile(List<string> AccumSeqStrings)
        {
            //                                      // -----------------------------------------------
            //                                      // write list of DNA sequence lines to a text file
            //                                      // -----------------------------------------------

            System.IO.StreamWriter OutputDNASeqBlclLnes = new System.IO.StreamWriter("c:\\temp\\DNASeqLines.txt");
            foreach (string AccumSeqBlckStrs in AccumSeqStrings)
            {
                OutputDNASeqBlclLnes.WriteLine(AccumSeqBlckStrs);
            }

            OutputDNASeqBlclLnes.Close();
        }

        public static void WriteDefnLnesToFile(List<string> AccumDefnLines)
        {
            //                                      // -----------------------------------------------
            //                                      // write list of DNA sequence lines to a text file
            //                                      // -----------------------------------------------

            System.IO.StreamWriter OutputDNADefnLnes = new System.IO.StreamWriter("c:\\temp\\DNADefnLines.txt");
            foreach (string AccumSeqBlckStrs in AccumDefnLines)
            {
                OutputDNADefnLnes.WriteLine(AccumSeqBlckStrs);
            }

            OutputDNADefnLnes.Close();
        }



        //                                      // filehandler class
    }

//                                      //namespace   
}
