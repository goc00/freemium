using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contract.Models
{
    public class NaranyaNotification
    {
        public string id_event { get; set; }
        public string ipn_url { get; set; }
        public string ipn_type { get; set; }
        public string verify_sign { get; set; }
        public string id_app { get; set; }
        public string id_customer { get; set; }
        public string id_transaction { get; set; }
        public int? amount { get; set; }
        public string currency { get; set; }
        public string id_subscription { get; set; }
        public string status { get; set; }
        public string id_service { get; set; }
        public int? created { get; set; }
        public int? updated { get; set; }

        public string GetUrlParams()
        {
            string resp = @"?id_event=" + this.id_event + "&ipn_url=" + this.ipn_url + "&ipn_type=" + this.ipn_type + "&verify_sign=" + this.verify_sign + "&id_app=" + this.id_app + "&id_customer=" + this.id_customer + "&id_subscription=" + this.id_subscription + "&id_service=" + this.id_service + "&status=" + this.status + "&created=" + this.created + "&id_transaction="+this.id_transaction+"&amount="+this.amount+"&currency="+this.currency;

            return resp;
        }

        public string GetUrlParamsDos()
        {
            string resp = @"?id_event=" + this.id_event+ "&ipn_url=" + this.ipn_url + "&ipn_type=" + this.ipn_type + "&verify_sign=" + this.verify_sign + "&id_app=" + this.id_app + "&id_customer=" + this.id_customer + "&id_transaction=" + this.id_transaction + "&amount=" + this.amount + "&currency=" + this.currency + "&id_subscription=" + this.id_subscription + "&status=" + this.status + "&id_service=" + this.id_service + "&created=" + this.created;

            return resp;
        }

        public string GetUrlParamsTres()
        {
            string resp = @"";

            if (!string.IsNullOrEmpty(this.id_event))
            {
                resp += "?id_event=" + this.id_event;
            }

            if (!string.IsNullOrEmpty(this.ipn_url))
            {
                resp += "&ipn_url=" + this.ipn_url;
            }

            if (!string.IsNullOrEmpty(this.ipn_type))
            {
                resp += "&ipn_type=" + this.ipn_type;
            }

            if (!string.IsNullOrEmpty(this.verify_sign))
            {
                resp += "&verify_sign=" + this.verify_sign;
            }

            if (!string.IsNullOrEmpty(this.id_app))
            {
                resp += "&id_app=" + this.id_app;
            }

            if (!string.IsNullOrEmpty(this.id_customer))
            {
                resp += "&id_customer=" + this.id_customer;
            }

            if (!string.IsNullOrEmpty(this.id_subscription))
            {
                resp += "&id_subscription=" + this.id_subscription;
            }

            if (!string.IsNullOrEmpty(this.id_service))
            {
                resp += "&id_service=" + this.id_service;
            }

            if (!string.IsNullOrEmpty(this.status))
            {
                resp += "&status=" + this.status;
            }

            if (this.created != null)
            {
                resp += "&created=" + this.created;
            }

            if (this.updated != null)
            {
                resp += "&updated=" + this.updated;
            }

            if (!string.IsNullOrEmpty(this.id_transaction))
            {
                resp += "&id_transaction=" + this.id_transaction;
            }

            if (this.amount != null)
            {
                resp += "&amount=" + this.amount;
            }

            if (!string.IsNullOrEmpty(this.currency))
            {
                resp += "&currency=" + this.currency;
            }


            return resp;

        }
    }
}