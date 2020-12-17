using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SecretSanta
{
    /// <summary>
    /// This program randomly selects the secret santa for a family and sends a text message to the giver who his/her receiver would be
    /// </summary>
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Family [] families = {
                new Family("XXXX", "","+14122322323"),
                new Family("BBBB", "","+13444544478"),
                
            };

            var familyWithReceivers = FigureOutReceivers(families);
            SendTextMessages(familyWithReceivers);
        }

        /// <summary>
        /// Returns the giver family and its randomly selected receiver.
        /// </summary>
        /// <param name="families">List of giver families</param>
        /// <returns>List of giver and receiver families</returns>
        private static Dictionary<Family, Family> FigureOutReceivers(IReadOnlyList<Family> families)
        {
            var familyWithReceivers = new Dictionary<Family, Family>();

            // Loop through each family and find its receiver family
            foreach (var family in families)
            {
                var r = new Random();

                while (true)
                {
                    var selectedIndex = r.Next(0, families.Count);

                    // if receiving family is already in the list keep going until you get a family that's not in list
                    if (familyWithReceivers.ContainsValue(families[selectedIndex]))
                    {
                        continue;
                    }

                    // check if receiver and giver are the same.
                    if (families[selectedIndex] == family) continue;

                    // then add it to the list, if the receiver family is not in list
                    familyWithReceivers.Add(family, families[selectedIndex]);
                    break;
                }
            }

            return familyWithReceivers;
        }

        /// <summary>
        /// send text message to each user with their secret santa receiver
        /// </summary>
        /// <remarks>https://www.twilio.com/docs/messaging/services/tutorials/how-to-send-sms-messages-services-csharp</remarks>
        /// <param name="familyWithReceivers"></param>
        private static void SendTextMessages(Dictionary<Family, Family> familyWithReceivers)
        {
            foreach (var familyWithReceiver in familyWithReceivers)
            {
                var body = $"{familyWithReceiver.Key.Name}. Your receiver family is {familyWithReceiver.Value.Name}. Check for mailing address on google sheets";
                var accountSid = "XXXXXXXXXX";
                var authToken = "BBBBBBBBB";

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: body,
                    @from: new Twilio.Types.PhoneNumber("+18787878787"),
                    to: new Twilio.Types.PhoneNumber(familyWithReceiver.Key.PhoneNo)
                );

                Console.WriteLine(message.Sid);
            }
        }

       
    }

    /// <summary>
    /// Class for storing family name address and phone no
    /// </summary>
    internal class Family 
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }

        public Family(string name, string address, string phoneNo)
        {
            Name = name;
            Address = address;
            PhoneNo = phoneNo;
        }
    }
}