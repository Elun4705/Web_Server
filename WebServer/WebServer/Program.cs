using Communications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Linq;

namespace WebServer
{
    /// <summary>
    /// Author:   H. James de St. Germain
    /// Date:     Spring 2020
    /// Updated:  Spring 2023
    /// 
    /// Code for a simple web server
    /// </summary>
    class WebServer
    {
        /// <summary>
        /// keep track of how many requests have come in.  Just used
        /// for display purposes.
        /// </summary>
        static private int counter = 1;

        public static Networking server = new(NullLogger.Instance, onConnect, OnDisconnect, onMessage, '\n');
        public static void onConnect(Networking n) { }



        public WebServer()
        {

        }
        public static void Main(string[] args)
        {
            server.WaitForClients(11001, true);
            Console.Read();
        }

        /// <summary>
        /// Basic connect handler - i.e., a browser has connected!
        /// Print an information message
        /// </summary>
        /// <param name="channel"> the Networking connection</param>

        internal static void OnClientConnect(Networking channel)
        {


            //throw new NotImplementedException("Print something about a connection happening");

        }

        /// <summary>
        /// Create the HTTP response header, containing items such as
        /// the "HTTP/1.1 200 OK" line.
        /// 
        /// See: https://www.tutorialspoint.com/http/http_responses.htm
        /// 
        /// Warning, don't forget that there have to be new lines at the
        /// end of this message!
        /// </summary>
        /// <param name="length"> how big a message are we sending</param>
        /// <param name="type"> usually html, but could be css</param>
        /// <returns>returns a string with the response header</returns>
        private static string BuildHTTPResponseHeader(int length, string type = "text/html")
        {
            return $"HTTP/1.1 200 OK\r\nConnection: Closed\r\nContent-Length: {length}\r\nContent-Type: {type}; charset-UTF-8\r\n";


            //throw new NotImplementedException("Modify this to return an actual HTTP protocol message");
        }

        /// <summary>
        ///   Create a web page!  The body of the returned message is the web page
        ///   "code" itself. Usually this would start with the doctype tag followed by the HTML element.  Take a look at:
        ///   https://www.sitepoint.com/a-basic-html5-template/
        /// </summary>
        /// <returns> A string the represents a web page.</returns>
        private static string BuildHTTPBody()
        {
            return @$"<!doctype html>
                        <html lang=""en"">
                            <head>
                                <meta charset=utf-8>
                                <meta name=viewport content=width=device-width, initial-scale=1>
                                <title> Agario game information showing bar</title>
                                <meta name=Webpage description content=A simple HTML5 Template for new projects.>
                                <meta name=lunaX content=work for database server>
                            </head>

                            <body>
                                <h1> Welcome to this WebPage</h1>
                                <p> there is data that might be accesss in below</p>
                                <p> Here is link reload this page:<a href= http://localhost:11001>Reload Page</a></p>
                            </body>
                        </html>";

        }
        /// <summary>
        /// the method is showing the highscore that in our databse
        /// </summary>
        /// <returns></returns>
        private static string HighScoresMessage()
        {
            try
            {
            string gethighscores = $@"<!doctype html>
                       <html lang=""en"">
                             <head>
                                <meta charset=utf-8>
                                <meta name=viewport content=width=device-width, initial-scale=1>
                                <title> Agario game players scores information</title>
                                <meta name=Webpage description content=showing the scores about this game.>
                                <meta name=lunaX content=work for database server>
                            </head>

                            <body>
                                <h1>Each player high score </h1>
                                <hr/>
                                <h2> game list</h2>
                                {DataBaseGetter.HighScore()}
                            </body>
                        </html>";
                return gethighscores;
            }
            catch
            {
                return $@"<!doctype html>
                       <html lang=""en"">
                             <head>
                                <meta charset=utf-8>
                                <meta name=viewport content=width=device-width, initial-scale=1>
                                <title> Agario game players scores information</title>
                                <meta name=Webpage description content=showing the scores about this game.>
                                <meta name=lunaX content=work for database server>
                            </head>

                            <body>
                                <h1>Each player high score </h1>
                                <hr/>
                                <h2> there is no highscores in database</h2>
                            </body>
                        </html>";
            }
        }
        /// <summary>
        /// the method showing particualr player scores that in our database
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string ScoreOfParticularPlayer(string name)
        {
            try
            {
            return $@"<!doctype html>
                       <html lang=""en"">
                             <head>
                                <meta charset=utf-8>
                                <meta name=viewport content=width=device-width, initial-scale=1>
                                <title> Agario game players scores information</title>
                                <meta name=Webpage description content=showing the scores about this game.>
                                <meta name=lunaX content=work for database server>
                            </head>

                            <body>
                                <h1> Score for a Particular Player</h1>
                                <hr/>
                                <h2> player score list </h2>
                                {DataBaseGetter.ParticularPlayerScore(name)}
                            </body>

";
            }
            catch
            {
                return $@"<!doctype html>
                       <html lang=""en"">
                             <head>
                                <meta charset=utf-8>
                                <meta name=viewport content=width=device-width, initial-scale=1>
                                <title> Agario game players scores information</title>
                                <meta name=Webpage description content=showing the scores about this game.>
                                <meta name=lunaX content=work for database server>
                            </head>

                            <body>
                                <h1>Each player high score </h1>
                                <hr/>
                                <h2> Particular scores is not in Database</h2>
                            </body>
                        </html>";
            }

        }
        /// <summary>
        /// this one is used to create a table in our database and showing whether creation is good
        /// </summary>
        /// <param name="checkGoodOrBadWeb"></param>
        /// <returns></returns>
        private static string createTable(bool checkGoodOrBadWeb)
        {
            if (checkGoodOrBadWeb)
            {
                return $@"<!doctype html>
                            <html lang=""en"">
                                    <head>
                                    <meta charset=utf-8>
                                    <meta name=viewport content=width=device-width, initial-scale=1>
                                    <title> Agario game players scores information</title>
                                    <meta name=Webpage description content=showing the scores about this game.>
                                    <meta name=lunaX content=work for database server>
                                </head>

                                <body>
                                    <h1> Score for a Particular Player</h1>
                                    <hr/>
                                    <h2> player score list </h2>
                                    <p>the data store into dataBase successfully </p>
                                </body>
        ";

            }
            else
            {
                return $@"<!doctype html>
                           <html lang=""en"">
                                 <head>
                                    <meta charset=utf-8>
                                    <meta name=viewport content=width=device-width, initial-scale=1>
                                    <title> Agario game players scores information</title>
                                    <meta name=Webpage description content=showing the scores about this game.>
                                    <meta name=lunaX content=work for database server>
                                </head>

                                <body>
                                    <h1> Score for a Particular Player</h1>
                                    <hr/>
                                    <h2> player score list </h2>
                                    <p>can not store data into database.there has already one table in database </p>
                                </body>

    ";

            }

        }
        /// <summary>
        /// this method is showing a fancy webpage that will help us see some different with others
        /// so, I return a table that store ID and related name.
        /// </summary>
        /// <returns></returns>
        private static string fancy()
        {
            try
            {
                DataBaseGetter.fancy();
                string text = $@"<!doctype html>
                               <html lang=""en"">
                                     <head>
                                        <meta charset=utf-8>
                                        <meta name=viewport content=width=device-width, initial-scale=1>
                                        <title> Agario game fancy page</title>
                                        <meta name=Webpage description content=showing players information.>
                                        <meta name=lunaX content=work for database server>
                                    </head>

                                    <body style=""background-color:grey;"">
                                        <h1 style=""background-color:yellow;""> fancy page about players</h1>
                                        <hr/>
                                        <h2 style=""background-color:green;""> players table </h2>
                                        <table style=""background-color:red;color:white"">
                                            <tr>  
                                                <th>PlayerID</th>  
                                                <th>Names</th>  
                                            </tr>";
                foreach (KeyValuePair<int, string> player in DataBaseGetter.Players)
                {
                    text += $@"{"\r\n"}<tr>
                                    <th>{player.Key}</th>
                                    <th>{player.Value}</th>
                               </tr>";

                }
                text += $@"{"\r\n"}</table>
                            </body>";
                return text;

            }
            catch
            {
                return $@"<!doctype html>
                               <html lang=""en"">
                                     <head>
                                        <meta charset=utf-8>
                                        <meta name=viewport content=width=device-width, initial-scale=1>
                                        <title> Agario game fancy page</title>
                                        <meta name=Webpage description content=showing players information.>
                                        <meta name=lunaX content=work for database server>
                                    </head>

                                    <body style=""background-color:grey;"">
                                        <h1 style=""background-color:yellow;""> fancy page about players</h1>
                                        <hr/>
                                        <h2 style=""background-color:green;""> this is fancy page :) </h2>
                                </body>";
            }
        }
        /// <summary>
        /// this method insert a bunch of data into database and showing whether the insertion is correct
        /// </summary>
        /// <returns></returns>
        private static string insertData()
        {
            return $@"<!doctype html>
                           <html lang=""en"">
                                 <head>
                                    <meta charset=utf-8>
                                    <meta name=viewport content=width=device-width, initial-scale=1>
                                    <title> showing insertion is correct</title>
                                    <meta name=Webpage description content=showing insertion correct message.>
                                    <meta name=lunaX content=work for database server>
                                </head>

                                <body>
                                    <h1> the message about whether insertion is correcct.</h1>
                                    <hr/>
                                    <h2> result for insertion </h2>
                                    <p>successful insert data into database </p>
                                </body>

    ";
        }
        /// <summary>
        /// this method showing our insertion is falied
        /// </summary>
        /// <returns></returns>
        private static string insertDataFailed()
        {
            return $@"<!doctype html>
                           <html lang=""en"">
                                 <head>
                                    <meta charset=utf-8>
                                    <meta name=viewport content=width=device-width, initial-scale=1>
                                    <title> showing insertion is failed</title>
                                    <meta name=Webpage description content=showing insertion failed message.>
                                    <meta name=lunaX content=work for database server>
                                </head>

                                <body>
                                    <h1> the message about whether insertion is correcct.</h1>
                                    <hr/>
                                    <h2> result for insertion </h2>
                                    <p>insertion is failed, please check inserted message </p>
                                </body>

    ";
        }


        /// <summary>
        /// Create a response message string to send back to the connecting
        /// program (i.e., the web browser).  The string is of the form:
        /// 
        ///   HTTP Header
        ///   [new line]
        ///   HTTP Body
        ///  
        ///  The Header must follow the header protocol.
        ///  The body should follow the HTML doc protocol.
        /// </summary>
        /// <returns> the complete HTTP response</returns>
        private static string BuildMainPage()
        {
            string message = BuildHTTPBody();
            string header = BuildHTTPResponseHeader(message.Length);

            string wholeMessage = header + "\r\n" + message;
            return wholeMessage;
        }



        /// <summary>
        ///   <para>
        ///     When a request comes in (from a browser) this method will
        ///     be called by the Networking code.  Each line of the HTTP request
        ///     will come as a separate message.  The "line" we are interested in
        ///     is a PUT or GET request.  
        ///   </para>
        ///   <para>
        ///     The following messages are actionable:
        ///   </para>
        ///   <para>
        ///      get highscore - respond with a highscore page
        ///   </para>
        ///   <para>
        ///      get favicon - don't do anything (we don't support this)
        ///   </para>
        ///   <para>
        ///      get scores/name - along with a name, respond with a list of scores for the particular user
        ///   <para>
        ///      get scores/name/highmass/highrank/startime/endtime - insert the appropriate data
        ///      into the database.
        ///   </para>
        ///   </para>
        ///   <para>
        ///     create - contact the DB and create the required tables and seed them with some dummy data
        ///   </para>
        ///   <para>
        ///     get index (or "", or "/") - send a happy home page back
        ///   </para>
        ///   <para>
        ///     get css/styles.css?v=1.0  - send your sites css file data back
        ///   </para>
        ///   <para>
        ///     otherwise send a page not found error
        ///   </para>
        ///   <para>
        ///     Warning: when you send a response, the web browser is going to expect the message to
        ///     be line by line (new line separated) but we use new line as a special character in our
        ///     networking object.  Thus, you have to send _every line of your response_ as a new Send message.
        ///   </para>
        /// </summary>
        /// <param name="network_message_state"> provided by the Networking code, contains socket and message</param>
        internal static void onMessage(Networking channel, string message)
        {
            // checking whether we received GET protocol.
            if (message.Contains("GET") || message.Contains("PUT"))
            {
                // check whether we received connection request.
                if (message.Contains("GET / HTTP/1.1"))
                {
                    channel.Send(BuildHTTPResponseHeader(BuildHTTPBody().Length));
                    channel.Send("");
                    channel.Send(BuildHTTPBody());
                    channel.Disconnect();
                }
                // checking whether we received highscores request and showing something on webpage
                else if (message.Contains("highscores"))
                {
                    var body = HighScoresMessage();
                    var header = BuildHTTPResponseHeader(body.Length);
                    channel.Send(header);
                    channel.Send("");
                    channel.Send(body);
                    channel.Disconnect();
                }
                // checking is our URL request is particualr scores request or endpoint request.
                // and then it will do related work for URL and showing something on webpage
                else if (message.Contains("scores"))
                {
                    message = message.Split(new[] { '\r', '\n' }).FirstOrDefault();
                    List<string> messageArray = message.Split(" ").ToList();
                    List<string> content = messageArray[1].Split('/').ToList();
                    if (content.Count == 3)
                    {
                        // this means we want to get particular player's score.
                        string name = content[2];
                        channel.Send(BuildHTTPResponseHeader(ScoreOfParticularPlayer(name).Length));
                        channel.Send("");
                        channel.Send(ScoreOfParticularPlayer(name));
                        channel.Disconnect();
                    }
                    // if that is endpoint request, we will showing that on webpage
                    else if (content.Count == 7)
                    {
                        try
                        {
                            DataBaseGetter.insertion(content);
                            channel.Send(BuildHTTPResponseHeader(insertData().Length));
                            channel.Send("");
                            channel.Send(insertData());
                            
                        }
                        catch
                        {
                            channel.Send(BuildHTTPResponseHeader(insertDataFailed().Length));
                            channel.Send("");
                            channel.Send(insertDataFailed());
                        }
                    }
                }
                // we create a table in database and showing whether the creation work
                else if (message.Contains("create"))
                {
                    string showingOnWebpage = "";
                    try
                    {
                        if (DataBaseGetter.createTable() == "" || DataBaseGetter.createTable() == "/")
                        {
                            bool GoodWeb = true;
                            showingOnWebpage = createTable(GoodWeb);
                        }

                    }
                    catch
                    {
                        bool BadWeb = false;
                        showingOnWebpage = createTable(BadWeb);
                    }
                    channel.Send(BuildHTTPResponseHeader(showingOnWebpage.Length));
                    channel.Send("");
                    channel.Send(showingOnWebpage);
                    channel.Disconnect();
                }
                // if that is fancy request, we will make a fancy webpage for user.
                else if (message.Contains("fancy"))
                {
                    var body = fancy();
                    var header = BuildHTTPResponseHeader(body.Length);
                    channel.Send(header);
                    channel.Send("");
                    channel.Send(body);
                    channel.Disconnect();
                }
                else
                {
                    SendCSSResponse();
                }

            }
        }

        /// <summary>
        /// Handle some CSS to make our pages beautiful
        /// </summary>
        /// <returns>HTTP Response Header with CSS file contents added</returns>
        private static string SendCSSResponse()
        {
            return $@"<!doctype html>
                           <html lang=""en"">
                                 <head>
                                    <meta charset=utf-8>
                                    <meta name=viewport content=width=device-width, initial-scale=1>
                                    <title> showing insertion is failed</title>
                                    <meta name=Webpage description content=showing insertion failed message.>
                                    <meta name=lunaX content=work for database server>
                                </head>

                                <body >
                                    <h1> CSS.</h1>
                                    <hr/>
                                    <h2> result for insertion </h2>
                                    <p>CSS string: you are good </p>
                                </body>

    ";
        }


        /// <summary>
        ///    (1) Instruct the DB to seed itself (build tables, add data)
        ///    (2) Report to the web browser on the success
        /// </summary>
        /// <returns> the HTTP response header followed by some informative information</returns>
        private static string CreateDBTablesPage()
        {
            return DataBaseGetter.createTable();
        }
        /// <summary>
        /// service for networking object. 
        /// </summary>
        /// <param name="channel"></param>
        internal static void OnDisconnect(Networking channel)
        {
            Debug.WriteLine($"Goodbye {channel.RemoteAddressPort}");
        }

    }
}