using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TT
{
    public class SendMsg
    {   
        private static int _uniqueid = 1;
        private string uniqueID()
        { return Convert.ToString(_uniqueid++); }

        public static QuickFix.SessionID priceSessionID;
        public static QuickFix.SessionID orderSessionID;

        /// <summary>
        /// Submit a FIX message to subscribe to gateway status.  Status is displayed but no action is attached.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="SubscriptionRequestType"></param>
        public void ttGatewayStatusRequest(char SubscriptionRequestType)
        {
            try
            {
                //Gateway Status Request
                QuickFix42.Message gsr = new QuickFix42.Message(new QuickFix.MsgType("UAR"));
                
                gsr.setChar(QuickFix.SubscriptionRequestType.FIELD, (sbyte)SubscriptionRequestType);

                gsr.setField(new TT.GatewayStatusReqId(uniqueID()));

                QuickFix.Session.sendToTarget(gsr, orderSessionID);
               
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Submit FIX request to retreive all the days trades, SODs and manual Fills
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reqtype"></param>
        public void ttRequestForPosition(int reqtype)
        {
            try
            {
                QuickFix42.Message rfp = new QuickFix42.Message(new QuickFix.MsgType("UAN"));

                rfp.setField(new TT.PosReqId(uniqueID()));
                rfp.setField(new TT.PosReqType(reqtype));

                QuickFix.Session.sendToTarget(rfp, orderSessionID);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// send a new order
        /// </summary>
        /// <param name="instrument">string array containing either
        /// {SecurityExchange, Symbol,SecurityType,MaturityMonthYear} or 
        /// {SecurityExchange, Symbol,SecurityID}</param>
        /// <param name="gateway"></param>
        /// <param name="account"></param>
        /// <param name="orderType"></param>
        /// <param name="tradePrice"></param>
        /// <param name="qty"></param>
        /// <param name="bs"></param>
        public void ttNewOrderSingle(string[] instrument, string gateway,
            string account, char orderType, double tradePrice, double qty, char bs)
        {
            //string[] instrument must contain:
            //{SecurityExchange, Symbol,SecurityType,MaturityMonthYear} or 
            //{SecurityExchange, Symbol,SecurityID}

            try
            {
                QuickFix42.NewOrderSingle nos = new QuickFix42.NewOrderSingle();

                nos.set(new QuickFix.ClOrdID(uniqueID()));

                #region DefineInstrument
                nos.set(new QuickFix.SecurityExchange(instrument[0]));
                nos.set(new QuickFix.Symbol(instrument[1]));

                if (instrument.GetLength(0) == 4)
                {
                    nos.set(new QuickFix.SecurityType(instrument[2]));
                    nos.set(new QuickFix.MaturityMonthYear(instrument[3]));
                }
                else if (instrument.GetLength(0) == 3)
                { nos.set(new QuickFix.SecurityID(instrument[2])); }
                else
                { throw new System.Exception("Incorrect parameters for insturment definition"); } 
                #endregion
              
                nos.set(new QuickFix.OrderQty(qty));
                nos.set(new QuickFix.Side(bs));
                nos.set(new QuickFix.Account(account));

                nos.set(new QuickFix.OrdType(orderType));
                if (orderType == QuickFix.OrdType.LIMIT)
                { nos.set(new QuickFix.Price(tradePrice)); }

                //Use this code if FA is setup to accept tag 47 and 204 instead of custom tag 18205
                //nos.set(new QuickFix.Rule80A(QuickFix.Rule80A.AGENCY_SINGLE_ORDER));
                //nos.set(new QuickFix.CustomerOrFirm(QuickFix.CustomerOrFirm.CUSTOMER));
                nos.setString(TT.TTAccountType.FIELD, TT.TTAccountType.A1);

                //To add a TT custom tag to a QuickFix Message you must use setField or similar
                //the set method of the QuickFix42 message only allows setting standard FIX 4.2 fields
                //required for environments with multiple gateways with same products
                if (gateway !=null)
                { nos.setString(TT.ExchangeGateway.FIELD, gateway); }

                nos.setString(TT.FFT2.FIELD, "FFT2");
                nos.setString(TT.FFT3.FIELD, "FFT3");

                QuickFix.Session.sendToTarget(nos, orderSessionID);
              
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Request security definiton to capture the exchange point value for the contract.
        /// </summary>
        /// <param name="SecEx">Exchange</param>
        /// <param name="symbol">Exchange symbol for contract</param>
        /// <param name="secID">unique identifier supplied by the exchnage for this contract 
        /// For eurex this is the ticker and expiration</param>
        public void ttSecurityDefinitionRequest(string SecEx, string symbol, string securityType, string securityID)
        {
            //It is recommended to include either a securityType or securityID; otherwise a very large list of
            //contracts will be retruned.
            try
            {
                QuickFix42.SecurityDefinitionRequest sdr = new QuickFix42.SecurityDefinitionRequest();

                sdr.set(new QuickFix.SecurityReqID(uniqueID()));

                sdr.setField(new TT.RequestTickTable("Y"));

                sdr.set(new QuickFix.SecurityExchange(SecEx));
                sdr.set(new QuickFix.Symbol(symbol));

                if (securityType != null)
                { sdr.set(new QuickFix.SecurityType(securityType)); }
                if (securityID != null)
                { sdr.set(new QuickFix.SecurityID(securityID)); }

                QuickFix.Session.sendToTarget(sdr, priceSessionID);

            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// The Security Status Request (e) message is used by FIX clients to request the
        /// current status of a particular product.
        /// </summary>
        /// <param name="SecEx"></param>
        /// <param name="symbol"></param>
        /// <param name="secID"></param>
        public void ttSecurityStatusRequest(string[] instrument, char reqType)
        {
            //string[] instrument must contain:
            //{SecurityExchange, Symbol,SecurityType,MaturityMonthYear} or 
            //{SecurityExchange, Symbol,SecurityID}
            try
            {
                QuickFix42.SecurityStatusRequest ssr = new QuickFix42.SecurityStatusRequest();

                ssr.set(new QuickFix.SecurityStatusReqID(uniqueID()));
                ssr.set(new QuickFix.SubscriptionRequestType(reqType));

                // Define instrument
                ssr.set(new QuickFix.SecurityExchange(instrument[0]));
                ssr.set(new QuickFix.Symbol(instrument[1]));

                if (instrument.GetLength(0) == 4)
                {
                    ssr.set(new QuickFix.SecurityType(instrument[2]));
                    ssr.set(new QuickFix.MaturityMonthYear(instrument[3]));
                }
                else if (instrument.GetLength(0) == 3)
                { ssr.set(new QuickFix.SecurityID(instrument[2])); }
                else
                { throw new System.Exception("Incorrect parameters for insturment definition"); }

                QuickFix.Session.sendToTarget(ssr, priceSessionID);

            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Request Bid and offer prices subscription for a specific contract.
        /// </summary>
        /// <param name="SecEx">Exchange</param>
        /// <param name="symbol">Exchange symbol for contract</param>
        /// <param name="secID">unique identifier supplied by the exchnage for this contract 
        /// For eurex this is the ticker and expiration</param>
        public void ttMarketDataRequest(string[] instrument, char reqType)
        {
            //string[] instrument must contain:
            //{SecurityExchange, Symbol,SecurityType,MaturityMonthYear} or 
            //{SecurityExchange, Symbol,SecurityID}

            try
            {
                QuickFix42.MarketDataRequest mdr = new QuickFix42.MarketDataRequest();

                mdr.set(new QuickFix.MDReqID(uniqueID()));

                mdr.set(new QuickFix.SubscriptionRequestType(reqType));
                mdr.set(new QuickFix.MDUpdateType(QuickFix.MDUpdateType.FULL_REFRESH)); //required if above type is SNAPSHOT_PLUS_UPDATES
                //1=Top of Book, 0 = full book, other integer equals number of levels
                mdr.set(new QuickFix.MarketDepth(1));
                mdr.set(new QuickFix.AggregatedBook(true));

                QuickFix42.MarketDataRequest.NoMDEntryTypes tgroup = new QuickFix42.MarketDataRequest.NoMDEntryTypes();
                tgroup.set(new QuickFix.MDEntryType(QuickFix.MDEntryType.BID));
                mdr.addGroup(tgroup);
                tgroup.set(new QuickFix.MDEntryType(QuickFix.MDEntryType.OFFER));
                mdr.addGroup(tgroup);
                tgroup.set(new QuickFix.MDEntryType(QuickFix.MDEntryType.TRADE));
                mdr.addGroup(tgroup);

                QuickFix42.MarketDataRequest.NoRelatedSym sgroup = new QuickFix42.MarketDataRequest.NoRelatedSym();

                // Define instrument
                sgroup.set(new QuickFix.SecurityExchange(instrument[0]));
                sgroup.set(new QuickFix.Symbol(instrument[1]));

                if (instrument.GetLength(0) == 4)
                {
                    sgroup.set(new QuickFix.SecurityType(instrument[2]));
                    sgroup.set(new QuickFix.MaturityMonthYear(instrument[3]));
                }
                else if (instrument.GetLength(0) == 3)
                { sgroup.set(new QuickFix.SecurityID(instrument[2])); }
                else
                { throw new System.Exception("Incorrect parameters for insturment definition"); }

                mdr.addGroup(sgroup);

                QuickFix.Session.sendToTarget(mdr, priceSessionID);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Request all working orders so that they may be canceled
        /// </summary>
        public void ttOrderStatusRequest()
        {
            try
            {
                QuickFix42.OrderStatusRequest osr = new QuickFix42.OrderStatusRequest();

                //filter by account - optional
                //osr.set(new QuickFix.Account("sl002004"));
                //omit this for order book download
                //osr.set(new QuickFix.ClOrdID("uniqueClOrdID"));
                //osr.set(new QuickFix.OrderID("TTORDERKEY"));

                QuickFix.Session.sendToTarget(osr, orderSessionID);


            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Send order cancel request message for order specified by TTOrderKey (QuickFix.OrderID)
        /// </summary>
        /// <param name="TTOrderkey"></param>
        public void ttOrderCancelRequest(string TTOrderkey)
        {
            try
            {
                QuickFix42.OrderCancelRequest ocr = new QuickFix42.OrderCancelRequest();

                ocr.set(new QuickFix.ClOrdID(uniqueID()));
                ocr.set(new QuickFix.OrderID(TTOrderkey));

                QuickFix.Session.sendToTarget(ocr, orderSessionID);

            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString());}
        }

        /// <summary>
        /// Cancel and replace an order
        /// </summary>
        /// <param name="SecEx"></param>
        /// <param name="symbol"></param>
        /// <param name="secID"></param>
        /// <param name="ttOrderKey"></param>
        /// <param name="price"></param>
        /// <param name="qty"></param>
        /// <param name="side"></param>
        /// <param name="orderType"></param>
        public void ttOrderCancelReplace(string[] instrument, string account,
            string ttOrderKey, double price, double qty, char side, char orderType)
        {            
            //string[] instrument must contain:
            //{SecurityExchange, Symbol,SecurityType,MaturityMonthYear} or 
            //{SecurityExchange, Symbol,SecurityID}

            try
            {
                QuickFix42.OrderCancelReplaceRequest ocrr = new QuickFix42.OrderCancelReplaceRequest();

                ocrr.set(new QuickFix.ClOrdID(uniqueID()));
                // Define instrument
                ocrr.set(new QuickFix.SecurityExchange(instrument[0]));
                ocrr.set(new QuickFix.Symbol(instrument[1]));

                if (instrument.GetLength(0) == 4)
                {
                    ocrr.set(new QuickFix.SecurityType(instrument[2]));
                    ocrr.set(new QuickFix.MaturityMonthYear(instrument[3]));
                }
                else if (instrument.GetLength(0) == 3)
                { ocrr.set(new QuickFix.SecurityID(instrument[2])); }
                else
                { throw new System.Exception("Incorrect parameters for instrument definition"); }

                ocrr.set(new QuickFix.OrderID(ttOrderKey));

                ocrr.set(new QuickFix.Account(account));
                ocrr.set(new QuickFix.Rule80A(QuickFix.Rule80A.AGENCY_SINGLE_ORDER));
                ocrr.set(new QuickFix.CustomerOrFirm(QuickFix.CustomerOrFirm.CUSTOMER));

                ocrr.set(new QuickFix.Price(price));
                ocrr.set(new QuickFix.OrderQty(qty));
                ocrr.set(new QuickFix.Side(side));
                ocrr.set(new QuickFix.OrdType(orderType));

                //change order to HOLD status
                //ocrr.set(new QuickFix.ExecInst(QuickFix.ExecInst.HELD.ToString()));
                //ocrr.set(new QuickFix.ExecInst(QuickFix.ExecInst.NOT_HELD.ToString()));

                QuickFix.Session.sendToTarget(ocrr, orderSessionID);

            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Logout message requires that you also add code to onLogout method of the QuickFix implementation; 
        /// Otherwise QuickFix will attempt to re-login.
        /// </summary>
        public void ttLogout()
        {
            
            try
            {
                QuickFix42.Logout lo = new QuickFix42.Logout();

                //optional text field
                lo.set(new QuickFix.Text("LOG OUT message sent"));

                QuickFix.Session.sendToTarget(lo, priceSessionID);
                QuickFix.Session.sendToTarget(lo, orderSessionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
