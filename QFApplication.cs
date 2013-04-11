/***************************************************************************
* Copyright (c) 2012 Trading Technologies International, Inc.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy 
* of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights 
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 * 
* The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
*
*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 *INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 *PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 *HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 *CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR 
 *THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 ***************************************************************************
 * $Date: 2009/12/04 15:00:00EST $
 * $Revision: 1.0 $
 ***************************************************************************/

using System;
using System.Windows.Forms;
using System.Collections;

namespace QuickFix
{
    public class QFApplication : QuickFix42.MessageCracker, QuickFix.Application
    {
        private static LOG.LogFiles log = new LOG.LogFiles();
        private Control _control = null;
        private string _password = "";
        private bool _resetSession = false;

        public QuickFix.SessionSettings _settings = null;
        public QuickFix.FileStoreFactory _storeFactory = null;
        public QuickFix.FileLogFactory _logFactory = null;
        public QuickFix42.MessageFactory _messageFactory = null;
        public QuickFix.ThreadedSocketInitiator _initiator = null;

        public QFApplication()
        {  
        }

        #region Custom Methods & Variables for Sample application  
        
        protected string _TTOrderKey = null;
        protected string _senderCompID = null;
        public ArrayList _contracts = new ArrayList();
        protected ArrayList _gateways = new ArrayList();
        protected double _bidPrice = 0.00;
        protected double _askPrice = 0.00;

        public QuickFix.SessionID _priceSession = null;
        public QuickFix.SessionID _orderSession = null;

        public delegate void ThreadSafeFormControl(string text);
        protected ThreadSafeFormControl _tsfc;
        public void registerFormController(ThreadSafeFormControl fc)
        {
            _tsfc += fc;
        }
        protected void updateDisplay(string s)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsfc, new Object[] { s });
            }
            else
            {
                _tsfc(s);
            }
        }

        public delegate void ThreadSafeFormDataControl();
        protected ThreadSafeFormDataControl _tsfdc;
        public void registerFormDataController(ThreadSafeFormDataControl fc)
        {
            _tsfdc += fc;
        }
        protected void updateFormData()
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsfdc);
            }
            else
            {
                _tsfdc();
            }
        }

        public string getLastSiteOrderkey()
        {
            return _TTOrderKey;
        }
        public string getSenderCompID()
        {
            return _senderCompID;
        }
        public string getContractMonth(int index)
        {
            if (_contracts.Count-1 >=index)
            { return _contracts[index].ToString(); }
            else
            { return null; }
        }
        public double[] getTradePrices()
        {
            double[] p = {_bidPrice , _askPrice };

            return  p;
        }
        public Object[] getAvailableGateways()
        {
            return _gateways.ToArray();
        }
        public Object[] getAvailableContracts()
        {
            return _contracts.ToArray();
        }

        #endregion

        /// <summary>
        /// Initiate a connection through QuickFix to the TT Fix Adapter
        /// </summary>
        /// <param name="cfg">configuration file name</param>
        /// <param name="p">password</param>
        /// <param name="r">reset sequence numbers - always true for this application</param>
        /// <param name="c">the mainForm control</param>
        public void initiate(string cfg, string p, bool r, Control c)
        {
            log.CreateLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), null);

            try
            {
                _password = p;
                _resetSession = r;
                _control = c;

                _settings = new QuickFix.SessionSettings(cfg);
                _storeFactory = new QuickFix.FileStoreFactory(_settings);
                _logFactory = new QuickFix.FileLogFactory(_settings);
                _messageFactory = new QuickFix42.MessageFactory();

                _initiator = new QuickFix.ThreadedSocketInitiator(this,
                    _storeFactory,
                    _settings,
                    _logFactory,
                    _messageFactory);

                _initiator.start();
            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        #region QuickFix Implementation

            public virtual void onCreate(QuickFix.SessionID session)
            {   
                //application created with only one SenderCompID 
                updateDisplay(string.Format("onCreate: {0}", session));

                if (string.Equals(session.getSessionQualifier().ToUpperInvariant(), "ORDER"))
                {
                    TT.SendMsg.orderSessionID = session;
                }

                if (string.Equals(session.getSessionQualifier().ToUpperInvariant(), "PRICE"))
                {
                    TT.SendMsg.priceSessionID = session;
                }
            }

            public virtual void onLogon(QuickFix.SessionID session)
            {
                //FIX4.2:SenderCompID->TargetCompID:Session Qualifier
                updateDisplay(string.Format("onLogon: {0}", session));

                if (string.Equals(session.getSessionQualifier().ToUpperInvariant(), "ORDER"))
                {
                    //Add code to execute upon Logon of OrderSession 
                }

                if (string.Equals(session.getSessionQualifier().ToUpperInvariant(), "PRICE"))
                {
                    //Add code to execute upon Logon of PriceSession 
                }
            }

            public virtual void onLogout(QuickFix.SessionID sessionID)
            {
                Console.WriteLine("onLogout: " + sessionID.toString());

                if (!_initiator.isLoggedOn())
                {
                    _initiator.stop(true);
                    _initiator.Dispose(true);
                    
                }
                else
                { Console.WriteLine("Initiator still logged in"); }
                
                updateDisplay("You have logged out.");
                updateDisplay("Application will not function until you restart.");
            }

            public virtual void toAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                Console.WriteLine("toAdmin: " + message);

                QuickFix.MsgType mt = new QuickFix.MsgType();
                message.getHeader().getField(mt);

                if (mt.getValue() == QuickFix.MsgType.LOGON)
                {
                    if (!_password.Equals(""))
                    {
                        message.setField(new QuickFix.RawData(_password));
                    }

                    if (_resetSession)
                    {
                        message.setField(new QuickFix.ResetSeqNumFlag(true));
                    }
                }
                else
                {
                    updateDisplay("toAdmin: "+DateTime.Now.ToString("hh:mm:ss.fff"));
                }
            }

            public virtual void toApp(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                Console.WriteLine("toApp: " + message);
                updateDisplay("Message Sent: toApp ");
            }

            public virtual void fromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                Console.WriteLine("fromAdmin: " + message);
                updateDisplay("fromAdmin: "+DateTime.Now.ToString("hh:mm:ss.fff"));
                
                crack(message, sessionID);
            }

            public virtual void fromApp(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                try
                {
                    QuickFix.MsgType msgType = new QuickFix.MsgType();
                    message.getHeader().getField(msgType);
                    string msgTypeValue = msgType.getValue();

                    log.WriteLog(string.Format("fromApp: {0}", msgTypeValue));

                    switch (msgTypeValue)
                    {
                        case "UAT":
                            onGatewayStatusMessage((QuickFix42.Message)message, sessionID);
                            break;
                        case "UAP":
                            onPositionReportMessage((QuickFix42.Message)message, sessionID);
                            break;

                        default:
                            crack(message, sessionID);
                            break;
                    }
                }
                catch (QuickFix.UnsupportedMessageType umt)
                {
                    log.WriteLog("UnsupportedMessageType: " + umt.Message);
                    parseMessage(message, sessionID);
                }
                catch (Exception ex)
                {
                    log.WriteLog(ex.ToString());
                }
            } 

        #endregion

        #region application-level messages
      
        public override void onMessage(QuickFix42.BusinessMessageReject message, QuickFix.SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix42.ExecutionReport message, QuickFix.SessionID session)
        {
            parseMessage(message, session);

            _TTOrderKey = message.getOrderID().ToString();
            updateDisplay(string.Format("Current TT Order key is: {0}", _TTOrderKey));

            updateFormData();
        }

        public override void onMessage(QuickFix42.MarketDataRequestReject message, SessionID session)
        {
            parseMessage(message, session);

        }

        //shows example of parsing a standard QuickFix group
        public override void onMessage(QuickFix42.MarketDataSnapshotFullRefresh message, SessionID session)
        {
            parseMessage(message, session);

            if (message.hasGroup(new QuickFix42.MarketDataSnapshotFullRefresh.NoMDEntries()))
            {
                QuickFix42.MarketDataSnapshotFullRefresh.NoMDEntries g = new QuickFix42.MarketDataSnapshotFullRefresh.NoMDEntries();
                updateDisplay(string.Format("Found {0} NoMDEntries groups", message.groupCount(QuickFix.NoMDEntries.FIELD)));
                QuickFix.NoMDEntries nEntries = new QuickFix.NoMDEntries();
                message.getField(nEntries);

                for (uint i = 1; i <= message.groupCount(QuickFix.NoMDEntries.FIELD); i++)
                {
                    message.getGroup(i, g);
                    updateDisplay(string.Format("mdEntryType: {0}", g.get(new QuickFix.MDEntryType())));
                    updateDisplay(string.Format("mdEntryPx:   {0}", g.get(new QuickFix.MDEntryPx())));
                    updateDisplay(string.Format("mdEntrySize: {0}", g.get(new QuickFix.MDEntrySize())));

                    if (g.get(new QuickFix.MDEntryType()).getValue() == QuickFix.MDEntryType.BID)
                    {
                        //string s = g.get(new QuickFix.MDEntryPx()).ToString();
                        _bidPrice = Convert.ToDouble(g.get(new QuickFix.MDEntryPx()).getValue());
                    }


                    if (g.get(new QuickFix.MDEntryType()).getValue() == QuickFix.MDEntryType.OFFER)
                    {
                        string s = g.get(new QuickFix.MDEntryPx()).ToString();
                        _askPrice = Convert.ToDouble(s);
                    }

                }
            }

            updateFormData();
        }

        public override void onMessage(QuickFix42.MarketDataIncrementalRefresh message, SessionID session)
        {
            parseMessage(message, session);
            updateFormData();
        }

        public override void onMessage(QuickFix42.OrderCancelReject message, SessionID session)
        {
            parseMessage(message, session);

        }

        public override void onMessage(QuickFix42.SecurityDefinition message, QuickFix.SessionID session)
        {
            parseMessage(message, session);

            string s = message.getMaturityMonthYear().ToString();
            if (!_contracts.Contains(s))
            {
                _contracts.Add(s);
            }

            updateFormData();
        }

        public override void onMessage(QuickFix42.SecurityStatus message, QuickFix.SessionID session)
        {
            parseMessage(message, session);
        }

        //Shows example of parsing a custom TT group
        public void onGatewayStatusMessage(QuickFix42.Message message, SessionID session)
        {
            parseMessage(message, session);

            if (message.hasGroup(TT.NoGatewayStatus.FIELD))
            {
                updateDisplay(string.Format("Found {0} NoGatewayStatus groups", message.groupCount(TT.NoGatewayStatus.FIELD)));
                QuickFix.Group g = new Group(18201, 18202, new int[] { 18202, 207, 18203, 18204, 58, 0 });

                for (uint i = 1; i <= message.groupCount(TT.NoGatewayStatus.FIELD); i++)
                {
                    message.getGroup(i, g);
                    updateDisplay(string.Format("GatewayStatus:      {0}", g.getField(new TT.GatewayStatus())));
                    updateDisplay(string.Format("SecurityExchange:   {0}", g.getField(new QuickFix.SecurityExchange())));
                    updateDisplay(string.Format("ExchangeGateway:    {0}", g.getField(new TT.ExchangeGateway())));
                    updateDisplay(string.Format("SubExchangeGateway: {0}", g.getField(new TT.SubExchangeGateway())));
                    if (g.isSetField(new QuickFix.Text()))
                    { updateDisplay(string.Format("Text: {0}", g.getField(new QuickFix.Text()))); }

                    string s = g.getField(new TT.ExchangeGateway()).ToString();
                    if (!_gateways.Contains(s))
                    { _gateways.Add(s); }
                }
            }

            updateFormData();
        }

        public void onPositionReportMessage(QuickFix42.Message message, SessionID session)
        {
            parseMessage(message, session);
        }
        
        #endregion
       
        #region Session Level Messages
       
        public override void onMessage(QuickFix42.Heartbeat message, SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix40.TestRequest message, SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix42.ResendRequest message, SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix42.Reject message, SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix40.SequenceReset message, SessionID session)
        {
            parseMessage(message, session);
        }
        
        #endregion

        //See onGatewayStatusMessage or MarketDataSnapshotFullRefresh for example of parsing a group
        public void parseMessage(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            try
            {
                updateDisplay(message.GetType().FullName);

                QuickFix.SenderSubID senderID = new SenderSubID();
                QuickFix.TargetSubID targetID = new TargetSubID();
                QuickFix.OnBehalfOfSubID oboID = new OnBehalfOfSubID();

                if (message.getHeader().isSetField(senderID))
                { updateDisplay(string.Format("SenderSubID: {0}", message.getHeader().getField(senderID))); }

                if (message.getHeader().isSetField(targetID))
                { updateDisplay(string.Format("TargetSubID: {0}", message.getHeader().getField(targetID))); }

                if (message.getHeader().isSetField(oboID))
                { updateDisplay(string.Format("OnBehalfOfSubID: {0}", message.getHeader().getField(oboID))); }

                foreach (QuickFix.Field f in message)
                {
                    updateDisplay(string.Format("TAG: {0} = {1}", f.getField(), f));
                }

            }
            catch (Exception ex)
            {
                updateDisplay(ex.ToString());
            }
        }
    }
}
