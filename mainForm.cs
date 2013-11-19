using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using LOG;
using System.Threading;
using System.Collections;

namespace FIX_TRADER
{
    public partial class Form1 : Form
    {
        private LOG.LogFiles log = new LogFiles();
        private QuickFix.QFApplication _qf;

        #region Custom  Variables for sample application
        private string _gw = "CME";
        private string _product = "ES";
        private string _account = "sl5302";
        private double _tradePrice = -1.00;
        private Object[] _availGatewayList = null;
        private Object[] _availContractList = null;
        private bool _availGatewayLoaded = false; 
        #endregion

        public Form1()
        {
            InitializeComponent();
            
        }

        #region Form-based Events

        private void main_Load(object sender, EventArgs e)
        {
            log.CreateLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), this.listBox1);

            this.Text += " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.cbxGateways.Text = _gw;
            this.txtTicker.Text = _product;
            this.txtAccount.Text = _account;

            //Required Code to have application automatically initiate a connection. 
            try
            {
                _qf = new QuickFix.QFApplication();
                _qf.registerFormController(log.WriteLog);
                _qf.registerFormDataController(updateParameters);

                _qf.initiate("ini.cfg", "12345678", true , this);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttLogout();
            log.CloseLog();
            log.CleanLog(3);
        }

        private void account_TextChanged(object sender, EventArgs e)
        {
            _account = txtAccount.Text.Trim();
        }

        private void txtTicker_TextChanged(object sender, EventArgs e)
        {
            _product = txtTicker.Text.Trim();
        }

        private void cbxGateways_TextChanged(object sender, EventArgs e)
        {
            _gw = cbxGateways.Text.Trim();
        }

        private void cbxBS_TextChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("cbxBS_TextChanged: " + cbxBS.Text);

            if (cbxBS.Text == "BUY")
            {
                if ((double)_qf.getTradePrices().GetValue(0) != 0.00)
                { _tradePrice = (double)_qf.getTradePrices().GetValue(0); }
            }
            else
            {
                if ((double)_qf.getTradePrices().GetValue(1) != 0.00)
                { _tradePrice = (double)_qf.getTradePrices().GetValue(1); }
            }
        } 

        #endregion

        private void updateParameters()
        {
            _gw = cbxGateways.Text.Trim();
            _product = txtTicker.Text.Trim();

            if (cbxBS.Text == "BUY")
            {
                if ((double)_qf.getTradePrices().GetValue(0) != 0.00)
                { _tradePrice = (double)_qf.getTradePrices().GetValue(0); }
            }
            else
            {
                if ((double)_qf.getTradePrices().GetValue(1) != 0.00)
                { _tradePrice = (double)_qf.getTradePrices().GetValue(1); }
            }

            _availGatewayList = _qf.getAvailableGateways();
            if (!_availGatewayLoaded && _availGatewayList.Length > 0)
            {
                _availGatewayLoaded = true;
                cbxGateways.Items.AddRange(_availGatewayList);
                cbxGateways.Sorted = true;
                cbxGateways.DropDownStyle = ComboBoxStyle.DropDownList;
                cbxGateways.SelectedIndex = cbxGateways.Items.IndexOf(_gw);
            }

            _availContractList = _qf.getAvailableContracts();
            if (_availContractList.Length > 0)
            {
                int i = cbxContracts.Items.IndexOf(cbxContracts.Text);
                cbxContracts.Items.Clear();
                cbxContracts.Items.AddRange(_availContractList);
                cbxContracts.Sorted = true;
                cbxContracts.DropDownStyle = ComboBoxStyle.DropDownList;
                cbxContracts.SelectedIndex = Math.Max(i,0);
            }

        }


        private void btnSecurityDefinitionRequest_Click(object sender, EventArgs e)
        {
            _qf._contracts.Clear();

            TT.SendMsg send = new TT.SendMsg();
            send.ttSecurityDefinitionRequest(_gw, _product, QuickFix.SecurityType.FUTURE, null);
            
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttLogout();
        }

        private void btnNewOrderSingle_Click(object sender, EventArgs e)
        {
            char bs;
            if (cbxBS.Text == "BUY")
            { bs = QuickFix.Side.BUY; }
            else
            { bs = QuickFix.Side.SELL; }

            char orderType;
            if (_tradePrice == -1)
            { orderType = QuickFix.OrdType.MARKET; }
            else
            {
                orderType = QuickFix.OrdType.LIMIT;
            }

            TT.SendMsg send = new TT.SendMsg();
            send.ttNewOrderSingle(new string[] {_gw,  _product, QuickFix.SecurityType.FUTURE, cbxContracts.Text} , _gw,
            _account, orderType, _tradePrice, 1.00, bs);
        }

        private void btnMarketDataRequest_Click(object sender, EventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttMarketDataRequest(new string[] { _gw, _product, QuickFix.SecurityType.FUTURE, cbxContracts.Text },
                QuickFix.SubscriptionRequestType.SNAPSHOT);
        }

        private void btnSecurityStatusRequest_Click(object sender, EventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttSecurityStatusRequest(new string[] { _gw, _product, QuickFix.SecurityType.FUTURE, cbxContracts.Text },
                QuickFix.SubscriptionRequestType.SNAPSHOT);
        }

        private void btnGatewayStatusRequest_Click(object sender, EventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttGatewayStatusRequest(QuickFix.SubscriptionRequestType.SNAPSHOT);
        }
        
        private void btnRequestForPosition_Click(object sender, EventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttRequestForPosition(TT.PosReqType.POSITIONS);
        }
        
        private void btnOrderStatusRequest_Click(object sender, EventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttOrderStatusRequest();
        }
        
        private void btnOrderCancelRequest_Click(object sender, EventArgs e)
        {
            TT.SendMsg send = new TT.SendMsg();
            send.ttOrderCancelRequest(_qf.getLastSiteOrderkey());
        }

        private void btnOrderCancelReplace_Click(object sender, EventArgs e)
        {
            char bs;
            if (cbxBS.Text == "BUY")
            { bs = QuickFix.Side.BUY; }
            else
            { bs = QuickFix.Side.SELL; }

            TT.SendMsg send = new TT.SendMsg();
            send.ttOrderCancelReplace(new string[] { _gw, _product, QuickFix.SecurityType.FUTURE, cbxContracts.Text },
                _account, _qf.getLastSiteOrderkey(),_tradePrice, (double)new System.Random().Next(1,10), bs,
                QuickFix.OrdType.LIMIT);
        }


    }
}
