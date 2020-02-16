using System;
using GraphQL;
using GraphQLParser;
using Octokit;
using Salesforce.Common;
using Salesforce.Force;
using System.Collections;
using System.Collections.Generic;

namespace StatusReport
{
    class Program
    {

        static int CompareDates(KeyValuePair<DateTime, string> a, KeyValuePair<DateTime, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        static bool ConfirmCompleted(string data)
        {
            const string Warning = "⚠️";
            const string Warning2 = ":warning:";
            const string NoEntry = "🚫";
            const string NoEntry2 = ":no_entry_sign:";

            if (data.Contains(Warning) || data.Contains(Warning2) || data.Contains(NoEntry) || data.Contains(NoEntry2))
                return false;

            const string GoodHeart = "💚";
            const string GoodCheck = "✅";
            if (data.Contains(GoodHeart) || data.Contains(GoodCheck))
                return true;

            return false;
        }

        static async System.Threading.Tasks.Task AppendTopFiveAsync(GitHubClient github, string repo_owner, string repo_name, string TopFiveText, string PAULSWARNING, string BuildComment)
        {
            Console.WriteLine("Loading " + repo_owner + " : " + repo_name + " Issues...");
            var GitHubRepo_Issues = await github.Issue.GetAllForRepository(repo_owner, repo_name);


            if (GitHubRepo_Issues.Count > 0)
            {
                foreach (Issue i in GitHubRepo_Issues)
                {
                    if (i.Title.Contains(TopFiveText))
                    {
                        Console.WriteLine(i.Title);
                        //Console.WriteLine(i.Body);
                        Console.WriteLine("# Comments: " + i.Comments);
                        //Console.WriteLine("# Comments URL: " + i.CommentsUrl);

                        var comments = await github.Issue.Comment.GetAllForIssue(repo_owner, repo_name, i.Number);
                        bool testcomment = false;
                        foreach (IssueComment ic in comments)
                        {
                            if (ic.Body.Contains(PAULSWARNING))
                            {
                                testcomment = true;
                            }
                        }

                        if (!testcomment)
                        {
                            await github.Issue.Comment.Create(repo_owner, repo_name, i.Number, BuildComment);
                        }

                    }

                }
            }
            else
            {
                Console.WriteLine("No Issues Retrived");
            }

        }

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Loading github...");
            
            //string secretkey = Environment.GetEnvironmentVariable("GITHUB_TOKEN"); //or is it secrets.GITHUB_TOKEN
            
            var github = new GitHubClient(new ProductHeaderValue("Pauliver-StatusReport"))
            {
                Credentials = new Credentials(args[0])
            };


            int ThisWeekNum = TimeHelpers.GetIso8601WeekOfYear(System.DateTime.Now);

            List<KeyValuePair<DateTime, String>> FollowUpActions = new List<KeyValuePair<DateTime, string>>();


            string repo_owner = args[1]; //"GitHub";
            string repo_name = args[2]; //"IRL";
            { // IRL issues


                var IRLRepo_Issues = await github.Issue.GetAllForRepository(repo_owner, repo_name);

                Console.WriteLine("Loading IRL Issues...");

                if (IRLRepo_Issues.Count > 0)
                {
                    foreach (Issue i in IRLRepo_Issues)
                    {
                        string issue_string = i.Body;
                        try
                        {
                            IRLissue irlissue = new IRLissue(i.Title,issue_string, i.HtmlUrl);
                            FollowUpActions.AddRange(irlissue.FollowUpActions);
                        }
                        catch(Exception ex)
                        {
                            Console.Write("Failed to Parse : " + i.Title + Environment.NewLine);
                            Console.Write(ex.StackTrace + Environment.NewLine);
                        }
                    }

                }
            }

            string PAULSWARNING = "--- **POSTED FROM PAULS SCRIPT** ---";

            string BuildComment = PAULSWARNING + Environment.NewLine + Environment.NewLine;
            string Build_Comment_Finished = "";

            { 
                repo_name = args[3]; //"Developer-Relations"
                FollowUpActions.Sort(CompareDates);

                {

                    BuildComment += "| Week | Issue | Status | Stage | Owner |" + Environment.NewLine;
                    BuildComment += "| ---- | ---- | ---- | ---- | ---- |" + Environment.NewLine;

                    Build_Comment_Finished += BuildComment;

                    foreach (var temp in FollowUpActions)
                    {
                        int WeekNum = TimeHelpers.GetIso8601WeekOfYear(temp.Key);
                        string tobeassigned = "| Week: " + WeekNum.ToString() + " " + temp.Value + Environment.NewLine;

                        string[] possiblyfinished = temp.Value.Split("|");

                        if (ConfirmCompleted(possiblyfinished[2]))
                            Build_Comment_Finished += tobeassigned;
                        else
                            BuildComment += tobeassigned;
                    }
                }
            }


            string TopFiveText = "TOP 5 Week " + (string)TimeHelpers.GetIso8601WeekOfYear(System.DateTime.Now).ToString();
            await AppendTopFiveAsync(github, repo_owner, repo_name, TopFiveText, PAULSWARNING, BuildComment);
            
        }
    }
}
