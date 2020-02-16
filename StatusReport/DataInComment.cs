using System;
using System.Collections.Generic;
using System.Text;

namespace StatusReport
{
    class DataInComment
    {
        string HintThereIsAComment = "<!-- $";
        string SingleDataComment = "<!-- $DataComment$ $KEY$=$VALUE$  -->";
        string EndDataComment = "<!-- /END -->";

        string DataComment_Org = "<!-- $OrgAndComms -->";
        string DataComment_Hiring = "<!-- $HiringStatus -->";
        string DataComment_TeamPosts = "< !-- $TeamPosts -->";
        string DataComment_StratFeatures = "< !-- $StratFeatures -->";
        string DataComment_OSMaint = "< !-- $OSMaint -->";
        string DataComment_OS = "< !-- $OS -->";
        string DataComment_MVP = "< !-- $MVP -->";
        string DataComment_DandD = "<!-- $DandD -->";
        string DataComment_Future = "< !-- $Future -->";
        string DataComment_Events = "< !-- $EventTasks -->";


        string data = null;

        System.Collections.Hashtable dict = new System.Collections.Hashtable();

        DataInComment(string text)
        {
            data = text;
        }


        string[] GetData()
        {
            string[] return_lines;
            string[] lines = data.Split(Environment.NewLine);
            if (!data.Contains(HintThereIsAComment))
                return null;


            return lines;
        }

        string GetText()
        {
            return data;
        }

        void AddComment(string key, string value)
        {
            dict[key] = value;
            data += SingleDataComment.Replace("$KEY$", key).Replace("$VALUE$", value); 
        }
    }
}
