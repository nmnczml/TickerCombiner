using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TickerCombiner.Model;
using Websocket.Client;

namespace TickerCombiner
{


    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                   
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();


                    Thread t1 = new Thread(() =>
                        {
                            gateIO(context, webSocket);


                        });

                    Thread t2 = new Thread(() =>
                        {
                            binance(context, webSocket);


                        });

                    t1.Start();
                    t2.Start();


                    t1.Join();


                    t2.Join();

                }
                else
                {
                    context.Response.StatusCode = 400;
                }

            });
            app.UseFileServer();
        }


        private string PrepareTickerString(string tickerStr, string market)
        {



            StringBuilder sb = new StringBuilder();

            foreach (string ticker in tickerStr.Split(';'))
            {
                if (!string.IsNullOrEmpty(ticker))
                {
                    try
                    {

                        string tickerMarket = ticker.Split('@')[0];
                        string symbol = ticker.Split('@')[1];
                        string fiat = ticker.Split('@')[2];

                        if (symbol.ToLower().Contains("usdt")
                            || symbol.ToLower().Contains("point")
                            || symbol.ToLower().Contains("usdttest"))
                            continue;

                        if (tickerMarket.Equals("b") && market.Equals("binance"))
                        {
                            sb.Append(symbol.ToLower() + fiat.ToLower() + "@miniTicker/");
                        }
                        else
                            if (tickerMarket.Equals("g") && market.Equals("gateio"))
                        {


                            sb.Append("\"" + symbol.ToUpper() + "_" + fiat.ToUpper() + "\",");
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }

            if (market.Equals("binance"))
            {

                string result = sb.ToString();
                if (result.Length > 0)
                    result = result.Substring(0, result.Length - 1);

                return result;
            }
            else if (market.Equals("gateio"))
            {

                string result = "[" + sb.ToString();
                if (result.Length > 0)
                    result = result.Substring(0, result.Length - 1);
                result = result + "]";
                return result;
            }

            return "";

        }


        private async Task binance(HttpContext context, WebSocket webSocket)
        {


            string ll = context.Request.Query["ll"].ToString();



            var tickerString = PrepareTickerString(ll, "binance");

            var url = new Uri("wss://stream.binance.com:9443/ws/" + tickerString);

            var exitEvent = new ManualResetEvent(false);

            using (var client = new WebsocketClient(url))
            {

                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client.ReconnectionHappened.Subscribe(info =>
                     Console.Write($"Reconnection happened, type: {info.Type}"));


                client.MessageReceived.Subscribe(msg =>
                handle(webSocket, msg, "binance")

                );

                client.Start();

                exitEvent.WaitOne();

            }

        }

        private object handle(WebSocket webSocket, ResponseMessage msg, string market)
        {

            if (market == "binance")
            {

                BinanceTicker ticker = JsonConvert.DeserializeObject<BinanceTicker>(msg.Text);

                ResultTicker result = new ResultTicker();
                result.c = ticker.c;
                result.E = ticker.E;
                result.e = ticker.e;
                result.h = ticker.h;
                result.l = ticker.l;
                result.m = "b";
                result.q = ticker.q;
                result.s = ticker.s;


                string sResult = JsonConvert.SerializeObject(result);

                webSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(sResult)),
                                  WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else if (market == "gateio")
            {
                try
                {





                    GateTicker ticker = JsonConvert.DeserializeObject<GateTicker>(msg.Text);

                    ResultTicker result = new ResultTicker();
                    result.c = ticker.result.last;
                    result.E = ticker.time;
                    result.e = ticker.@event;
                    result.h = ticker.result.high_24h;
                    result.l = ticker.result.low_24h;
                    result.m = "g";
                    result.q = ticker.result.quote_volume;
                    result.s = ticker.result.currency_pair;


                    string sResult = JsonConvert.SerializeObject(result);

                    webSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(sResult)),
                                      WebSocketMessageType.Text, true, CancellationToken.None);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return null;



        }

        private async Task gateIO(HttpContext context, WebSocket webSocket)
        {
            string ll = context.Request.Query["ll"].ToString();


            var tickerString = PrepareTickerString(ll, "gateio");

            var url = new Uri("wss://api.gateio.ws/ws/v4/");
            var exitEvent = new ManualResetEvent(false);

            using (var client = new WebsocketClient(url))
            {

                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client.ReconnectionHappened.Subscribe(info =>
                     Console.Write($"Reconnection happened, type: {info.Type}"));


                //string msgJSON = " {\"channel\": \"spot.tickers\",\"event\": \"subscribe\",\"payload\": [\"BTC_USDT\", \"XRP_USDT\"]}";
                string msgJSON = " {\"channel\": \"spot.tickers\",\"event\": \"subscribe\",\"payload\": " + tickerString + "}";

                client.Send(msgJSON);

                client.MessageReceived.Subscribe(msg =>
                                   handle(webSocket, msg, "gateio")



                );




                client.Start();



                exitEvent.WaitOne();

            }




        }


        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

    }



}
