using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//                                      // For the database connections and objects.
using System.Data.SqlClient;      

namespace DNAbstract
{
    class DNAbstract
    {
        static int Main(string[] args)
        {
            //                                      // -----------------------------------------------------------------------
            //                                      // verify the file given as an argument has at least one record
            //                                      // a definition type record as the start of the DNA data 
            //                                      // (although its not special)
            //                                      // -----------------------------------------------------------------------
            
            //                                      ? file name specified            
            if (args.Length > 0)
            {
                fileHandler checkHeader = new fileHandler();

                if (checkHeader.fH_openAndCheckHeader(args[0]))
                {
                }
                else
                {
                    Console.WriteLine("DNA Sequence data file has an invalid first line");
                }
            }
            else
            {
                Console.WriteLine("DNA Sequence data file name required");

            }

            //                                      // --------------------------------------------
            //                                      // get summary info about the DNA sequence data
            //                                      // --------------------------------------------
            fileHandler CountDefinitionLines = new fileHandler();

            int defnLineCount = CountDefinitionLines.fH_CountDefinitionLines(args[0]);

            Console.WriteLine("File contains " + defnLineCount.ToString() + " definiton lines");

            //                                      // ----------------------------------------------------
            //                                      // process the DNA sequence data into a list of strings
            //                                      // ----------------------------------------------------

            //                                      // create a file reader
            System.IO.StreamReader DNASeqFileReader = new System.IO.StreamReader(args[0]);

            //                                      // create a file handler
            fileHandler ProcessFile = new fileHandler();



            //                                      // create a store for the results from parsing a block of one or 
            //                                      // more DNA sequence line(s)
            ParseDefnRslt rsltFromDefnParse = new ParseDefnRslt();
           
            //                                      // A list of defintion line details 
            List<string> DefnLIneDetails = new List<string>();
 
            //                                      //           
            string mN_ParsedSeqDtls;
            string mN_ParseSeqMsg;
            //                                      // A list of defintion line details 
            List<string> DNASeqStrings = new List<string>();
            //                                      // a store to accumulate the Seq lines in
            StringBuilder seqLineStore = new StringBuilder();

            //                                      // A var to indicate what type record type is expected next
            //                                      // intialised to suit the intial position after reading the header
            //string currentLineType = "Definition";

            //                                      // Posit function will succeed
            int mn_RetVal = 0;

            //                                      // flag the very first definition line
            bool frstEverDefnLine = true;

            //                                      // a file line store
            string mN_FileLine;
            //                                      // a store of the last reacord read 
            string lastRecRead = ""; 

            //                                      // -------------------------------------------
            //                                      // Main loo loop through all lines in the file 
            //                                      // -------------------------------------------
            while ((mN_FileLine = DNASeqFileReader.ReadLine()) != null)
            {
                //                                      // save a copy of the reacord read
                lastRecRead = mN_FileLine;

                //                                      //
                rsltFromDefnParse = ProcessFile.fH_ParseDefnLine(mN_FileLine);

                if (rsltFromDefnParse.ParsedDefnStat == true)
                {
                    //                                      // Add defintion line detail to list
                    DefnLIneDetails.Add(rsltFromDefnParse.ParsedDefnDtls);
                    //                                      // ? first ever defn line
                    if (frstEverDefnLine)
                    {
                        // do nothing - no sequence block will have been accumulated - just reset the flag
                        frstEverDefnLine = false;
                    }
                    else
                    {
                        //                                      // A block has been completed - add it to the list
                        DNASeqStrings.Add(seqLineStore.ToString());
                        //                                      // reset the store of acumulated sequence lines
                        seqLineStore.Clear();
                    }
                                                             
                }
                else
                {
//                                      // ? parsing and validation of the DNA sequence records Ok

                    if (ProcessFile.fH_ParseSeqBlock(mN_FileLine,
                                                    DNASeqFileReader,
                                                    out mN_ParsedSeqDtls,
                                                    out mN_ParseSeqMsg) == true)
                    {
                        //                                      // Add the conatenated seq block lines string into the list of 
                        //                                      // DNA seq strings 
                        seqLineStore.Append(mN_ParsedSeqDtls);
                    }
                    else
                    {
                        //                                      // unknown line type
                        Console.WriteLine(mN_ParseSeqMsg);
                        return(1);
                    }

                }

            //                                      // end while 
            }

            //                                      // ? last line was seq line
            if (ProcessFile.fH_ParseSeqBlock(lastRecRead,
                                                    DNASeqFileReader,
                                                    out mN_ParsedSeqDtls,
                                                    out mN_ParseSeqMsg) == true)
            {
                //                                      //add the final set of seq lines to the list
                DNASeqStrings.Add(seqLineStore.ToString());
            }
            else
            {
                mn_RetVal = 2;
            }

            //                                      // -----------------------------------------------
            //                                      // write DNA definition and sequence data to files
            //                                      // -----------------------------------------------
            fileHandler.WriteDefnLnesToFile(DefnLIneDetails);
            fileHandler.WriteSeqLnesToFile(DNASeqStrings);

            //                                      // quit program 
            return (mn_RetVal);
        }

        public class ParseDefnRslt
        {
            public bool ParsedDefnStat;
            public string ParsedDefnDtls;
            public string ParseDefnMsg;
        }
    }
}
