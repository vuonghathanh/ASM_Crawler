﻿using HtmlAgilityPack;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UrlCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            StartCrawlerUrlAsync();
            Console.ReadLine();
        }

        private static async Task StartCrawlerUrlAsync()
        {
            var url = "https://vnexpress.net/suc-khoe/dinh-duong";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var articles = htmlDocument.DocumentNode.Descendants("article").ToList();
            foreach (var article in articles)
            {
                if(article.Descendants("a").FirstOrDefault() != null)
                {
                    var link = (string)article.Descendants("a").FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value;
                    var factory = new ConnectionFactory() { HostName = "localhost" };

                    var connection = factory.CreateConnection();

                    var channel = connection.CreateModel();
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                   
                        
                       
                        var body = Encoding.UTF8.GetBytes(link);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "amqAsmWCF",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", link);
                    }
                }
                
                

                
                
            }
        }
    }
