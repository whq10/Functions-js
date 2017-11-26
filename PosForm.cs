using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.PointOfService;

namespace Hotpot
{
    public partial class PosForm : Form
    {

        private PosExplorer explorer;
        private DeviceCollection scannerList;
        private DeviceCollection scannerList1;
        private Scanner activeScanner;

        bool setQuantity; // false;
        bool setPeopleQuantity; // false;


        public PosForm()
        {
            InitializeComponent();
        }

        private void ScannerLab1_Load(object sender, EventArgs e)
        {
            explorer = new PosExplorer();

            scannerList1 = explorer.GetDevices();
            bsDevices.DataSource = scannerList1;
            cboDevices.DisplayMember = scannerList1.ToString();

            scannerList = explorer.GetDevices(DeviceType.Scanner);
            devicesBindingSource.DataSource = scannerList;
            cboDevices.DisplayMember = scannerList.ToString();

        }

        private void btnActivateDevice_Click(object sender, EventArgs e)
        {
            if (lstDevices.SelectedItem != null)
            {
                ActivateScanner((DeviceInfo)lstDevices.SelectedItem);
            }
        }

        private void reportFailure()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void ActivateScanner(DeviceInfo selectedScanner)
        {
            //Verifify that the selectedScanner is not null
            // and that it is not the same scanner already selected
            if (selectedScanner != null && !selectedScanner.IsDeviceInfoOf(activeScanner))
            {
                // Configure the new scanner
                DeactivateScanner();

                // Activate the new scanner
                UpdateEventHistory(string.Format("Activate Scanner: {0}", selectedScanner.ServiceObjectName));
                try
                {
                    activeScanner = (Scanner)explorer.CreateInstance(selectedScanner);
                    activeScanner.Open();
                    activeScanner.Claim(1000);
                    activeScanner.DeviceEnabled = true;
                    activeScanner.DataEvent += new DataEventHandler(activeScanner_DataEvent);
                    activeScanner.ErrorEvent += new DeviceErrorEventHandler(activeScanner_ErrorEvent);
                    activeScanner.DecodeData = true;
                    activeScanner.DataEventEnabled = true;
                }
                catch (PosControlException)
                {
                    // Log error and set the active scanner to none
                    UpdateEventHistory(string.Format("Activation Failed: {0}", selectedScanner.ServiceObjectName));
                    activeScanner = null;
                }
            }
        }


        private void DeactivateScanner()
        {
            if (activeScanner != null)
            {
                // We have an active scanner, lets log that we are
                // about to close it.
                UpdateEventHistory("Deactivate Current Scanner");

                try
                {
                    // Close the active scanner
                    activeScanner.Close();
                }
                catch (PosControlException)
                {
                    // Log any error that happens
                    UpdateEventHistory("Close Failed");
                }
                finally
                {
                    // Don't forget to set activeScanner to null to
                    // indicate that we no longer have an active
                    // scanner configured.
                    activeScanner = null;
                }
            }
        }
        void activeScanner_DataEvent(object sender, DataEventArgs e)
        {
            UpdateEventHistory("Data Event");
            ASCIIEncoding encoder = new ASCIIEncoding();
            try
            {
                // Display the ASCII encoded label text
                txtScanDataLabel.Text = encoder.GetString(activeScanner.ScanDataLabel);
                // Display the encoding type
                txtScanDataType.Text = activeScanner.ScanDataType.ToString();

                // re-enable the data event for subsequent scans
                activeScanner.DataEventEnabled = true;
            }
            catch (PosControlException)
            {
                // Log any errors
                UpdateEventHistory("DataEvent Operation Failed");
            }
        }

        void activeScanner_ErrorEvent(object sender, DeviceErrorEventArgs e)
        {
            UpdateEventHistory("Error Event");

            try

            {
                // re-enable the data event for subsequent scans

                activeScanner.DataEventEnabled = true;
            }

            catch (PosControlException)
            {
                // Log any errors
                UpdateEventHistory("ErrorEvent Operation Failed");
            }
        }

        private void UpdateEventHistory(String newEvent)
        {
            txtEventHistory.Text = newEvent + System.Environment.NewLine + txtEventHistory.Text;
        }

        private void numberPress(char theNewCharacter)
        {
            if (this.setQuantity == false && this.setPeopleQuantity == false)
            {
                textBox3.Text += theNewCharacter;
            }
            else if (this.setQuantity == true && this.setPeopleQuantity == false)
            {
                textBox4.Text += theNewCharacter;
            }
            else
            {
                textBox5.Text += theNewCharacter;

            }
            return;
        }

        private void button_7_Click(object sender, EventArgs e)
        {
            this.numberPress('7');
            return;

        }

        private void setQuantityButton_Click(object sender, EventArgs e)
        {
            if (this.setQuantity == true)
            {
                this.setQuantity = false;
            }
            else
            {
                this.setQuantity = true;
            }
            return;
        }

        private void button_8_Click(object sender, EventArgs e)
        {
            this.numberPress('8');
            return;
        }

        private void button_9_Click(object sender, EventArgs e)
        {
            this.numberPress('9');
            return;
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            this.numberPress('4');
            return;
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            this.numberPress('5');
            return;
        }

        private void button_6_Click(object sender, EventArgs e)
        {
            this.numberPress('6');
            return;
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            this.numberPress('1');
            return;
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            this.numberPress('2');
            return;
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            this.numberPress('3');
            return;
        }

        private void button_0_Click(object sender, EventArgs e)
        {
            this.numberPress('0');
            return;
        }

        private void button_00_Click(object sender, EventArgs e)
        {
            this.numberPress('0');
            this.numberPress('0');
            return;
        }

        private void button_dot_Click(object sender, EventArgs e)
        {
            this.numberPress('.');
            return;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            if (this.setPeopleQuantity == false)
            {
                if (this.setQuantity == false)
                {
                    if (textBox3.Text.Length > 0)
                    {
                        char[] theReplacement = textBox3.Text.ToCharArray();
                        String theNewString = new String(theReplacement, 0, textBox3.Text.Length - 1);
                        textBox3.Text = theNewString;
                    }
                }
                else
                {
                    if (textBox4.Text.Length > 0)
                    {
                        char[] theReplacement = textBox4.Text.ToCharArray();
                        String theNewString = new String(theReplacement, 0, textBox4.Text.Length - 1);
                        textBox4.Text = theNewString;
                    }
                }

            }
            else
            {
                if (textBox5.Text.Length > 0)
                {
                    char[] theReplacement = textBox5.Text.ToCharArray();
                    String theNewString = new String(theReplacement, 0, textBox5.Text.Length - 1);
                    textBox5.Text = theNewString;
                }
            }
           
            return;
        }

        private void button_enter_Click(object sender, EventArgs e)
        {
            double sum = Convert.ToInt64(textBox4.Text) * Convert.ToDouble(textBox3.Text);

            sum *= 1.13;

            label8.Text = "总价： = $" + sum.ToString();

            label9.Text = "GST： = $" + (sum * 5 / 100).ToString();

            label10.Text = "PST： = $" + (sum * 8 / 100).ToString();

            label11.Text = "人数：" + Convert.ToInt64(textBox5.Text).ToString();

            label12.Text = "人均： = $" + (sum / Convert.ToInt64(textBox4.Text)).ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.setPeopleQuantity == true)
            {
                this.setPeopleQuantity = false;
            }
            else
            {
                this.setPeopleQuantity = true;
            }
            return;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = null;
            this.textBox4.Text = null;
            this.textBox5.Text = null;
            this.label8.Text = null;
            this.label9.Text = null;
            this.label10.Text = null;
            this.label11.Text = null;
            this.label12.Text = null;


        }
    }
}
