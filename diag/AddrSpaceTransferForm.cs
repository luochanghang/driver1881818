using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Jungo.wdapi_dotnet;
using Jungo.pcie_lib;
using wdc_err = Jungo.wdapi_dotnet.WD_ERROR_CODES;    
using BOOL = System.Boolean;
using BYTE = System.Byte;
using WORD = System.UInt16;
using DWORD = System.UInt32;
using UINT32 = System.UInt32;
using UINT64 = System.UInt64;

namespace Jungo.pcie_diag
{
    public class AddrSpaceTransferForm : System.Windows.Forms.Form
    {
        private Exception m_excp;  //�쳣����
        private PCIE_Device m_device;
        private object[] m_obj;
        private RW m_direction; //��д���Ͷ���
        private DWORD m_dwBar;
        private WDC_ADDR_MODE m_mode;
        private DWORD m_dwBytes;
        private TRANSFER_TYPE m_type;
        private DWORD m_dwOffset;
        private bool m_bAutoInc;
        private byte[] m_bData;
        private WORD[] m_wData;
        private UINT32[] m_u32Data;
        private UINT64[] m_u64Data;
        private System.Windows.Forms.ComboBox cmboBar;
        private System.Windows.Forms.ComboBox cmboMode;
        private System.Windows.Forms.TextBox txtOffset;
        private System.Windows.Forms.ComboBox cmboTransType;
        private System.Windows.Forms.TextBox txtNumBytes;
        private System.Windows.Forms.Label lblNumBytes;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblBar;
        private System.Windows.Forms.Label lblTransType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkBoxInc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btWrite;
        private System.Windows.Forms.Button btLog;
        private System.Windows.Forms.Button btRead;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

        private System.ComponentModel.Container components = null;

        public AddrSpaceTransferForm(PCIE_Device dev, string[] sBars)//���캯��
        {
            InitializeComponent();

            m_device = dev;

            for(int i=0; i<sBars.Length; ++i)
            {
                cmboBar.Items.Add(sBars[i]);  //�ռ�����
            }

            this.Text = "Read/Write Address Space Form";

            cmboMode.Items.AddRange(new object[]{"8 bits", "16 bits", "32 bits", "64 bits"});
            cmboTransType.Items.AddRange(new object[]{"block", "non-block"});
            chkBoxInc.Enabled = false;//��ѡ��
            txtNumBytes.Enabled = false;
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }
        //�������ɳ�ʼ������
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.cmboBar = new System.Windows.Forms.ComboBox();
            this.cmboMode = new System.Windows.Forms.ComboBox();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.cmboTransType = new System.Windows.Forms.ComboBox();
            this.txtNumBytes = new System.Windows.Forms.TextBox();
            this.lblNumBytes = new System.Windows.Forms.Label();
            this.lblOffset = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.btRead = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btLog = new System.Windows.Forms.Button();
            this.btWrite = new System.Windows.Forms.Button();
            this.lblBar = new System.Windows.Forms.Label();
            this.lblTransType = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkBoxInc = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmboBar
            // 
            this.cmboBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboBar.Location = new System.Drawing.Point(8, 32);
            this.cmboBar.Name = "cmboBar";
            this.cmboBar.Size = new System.Drawing.Size(280, 21);
            this.cmboBar.TabIndex = 0;
            this.cmboBar.Text = "---- ----";
            this.cmboBar.SelectedIndexChanged += new System.EventHandler(this.cmboBar_SelectedIndexChanged);
            // 
            // cmboMode
            // 
            this.cmboMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboMode.Location = new System.Drawing.Point(8, 96);
            this.cmboMode.Name = "cmboMode";
            this.cmboMode.Size = new System.Drawing.Size(121, 21);
            this.cmboMode.TabIndex = 1;
            this.cmboMode.Text = "---- ----";
            this.cmboMode.SelectedIndexChanged += new System.EventHandler(this.cmboMode_SelectedIndexChanged);
            // 
            // txtOffset
            // 
            this.txtOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOffset.Location = new System.Drawing.Point(24, 160);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(100, 20);
            this.txtOffset.TabIndex = 5;
            this.txtOffset.TextChanged += new System.EventHandler(this.txtOffset_TextChanged);
            // 
            // cmboTransType
            // 
            this.cmboTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboTransType.ItemHeight = 13;
            this.cmboTransType.Location = new System.Drawing.Point(168, 96);
            this.cmboTransType.Name = "cmboTransType";
            this.cmboTransType.Size = new System.Drawing.Size(104, 21);
            this.cmboTransType.TabIndex = 2;
            this.cmboTransType.Text = "---- ----";
            this.cmboTransType.SelectedIndexChanged += new System.EventHandler(this.cmboTransType_SelectedIndexChanged);
            // 
            // txtNumBytes
            // 
            this.txtNumBytes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumBytes.Location = new System.Drawing.Point(336, 48);
            this.txtNumBytes.Name = "txtNumBytes";
            this.txtNumBytes.Size = new System.Drawing.Size(100, 20);
            this.txtNumBytes.TabIndex = 3;
            this.txtNumBytes.TextChanged += new System.EventHandler(this.txtNumBytes_TextChanged);
            // 
            // lblNumBytes
            // 
            this.lblNumBytes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumBytes.Location = new System.Drawing.Point(328, 24);
            this.lblNumBytes.Name = "lblNumBytes";
            this.lblNumBytes.Size = new System.Drawing.Size(120, 23);
            this.lblNumBytes.TabIndex = 5;
            this.lblNumBytes.Text = "Number of Bytes (hex):";
            // 
            // lblOffset
            // 
            this.lblOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffset.Location = new System.Drawing.Point(8, 136);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(100, 23);
            this.lblOffset.TabIndex = 6;
            this.lblOffset.Text = "Offset (hex):";
            this.lblOffset.Click += new System.EventHandler(this.lblOffset_Click);
            // 
            // txtData
            // 
            this.txtData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtData.Location = new System.Drawing.Point(24, 186);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtData.Size = new System.Drawing.Size(511, 154);
            this.txtData.TabIndex = 15;
            this.txtData.TextChanged += new System.EventHandler(this.txtData_TextChanged);
            // 
            // btRead
            // 
            this.btRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRead.Location = new System.Drawing.Point(488, 24);
            this.btRead.Name = "btRead";
            this.btRead.Size = new System.Drawing.Size(75, 23);
            this.btRead.TabIndex = 7;
            this.btRead.Text = "Read";
            this.btRead.Click += new System.EventHandler(this.btRead_Click);
            // 
            // btExit
            // 
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExit.Location = new System.Drawing.Point(24, 136);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 11;
            this.btExit.Text = "Exit";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btLog);
            this.groupBox1.Controls.Add(this.btWrite);
            this.groupBox1.Controls.Add(this.btExit);
            this.groupBox1.Location = new System.Drawing.Point(464, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(120, 168);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // btLog
            // 
            this.btLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLog.Location = new System.Drawing.Point(23, 96);
            this.btLog.Name = "btLog";
            this.btLog.Size = new System.Drawing.Size(75, 23);
            this.btLog.TabIndex = 9;
            this.btLog.Text = "Clear Log";
            this.btLog.Click += new System.EventHandler(this.btLog_Click);
            // 
            // btWrite
            // 
            this.btWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btWrite.Location = new System.Drawing.Point(24, 56);
            this.btWrite.Name = "btWrite";
            this.btWrite.Size = new System.Drawing.Size(75, 23);
            this.btWrite.TabIndex = 8;
            this.btWrite.Text = "Write";
            this.btWrite.Click += new System.EventHandler(this.btWrite_Click);
            // 
            // lblBar
            // 
            this.lblBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBar.Location = new System.Drawing.Point(8, 8);
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size(120, 23);
            this.lblBar.TabIndex = 11;
            this.lblBar.Text = "Address Bar";
            // 
            // lblTransType
            // 
            this.lblTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransType.Location = new System.Drawing.Point(168, 72);
            this.lblTransType.Name = "lblTransType";
            this.lblTransType.Size = new System.Drawing.Size(120, 23);
            this.lblTransType.TabIndex = 12;
            this.lblTransType.Text = "Transfer Type";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "Transfer Mode";
            // 
            // chkBoxInc
            // 
            this.chkBoxInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxInc.Location = new System.Drawing.Point(328, 80);
            this.chkBoxInc.Name = "chkBoxInc";
            this.chkBoxInc.Size = new System.Drawing.Size(104, 24);
            this.chkBoxInc.TabIndex = 4;
            this.chkBoxInc.Text = "AutoIncrement Address";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(304, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(152, 104);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Block Transfer Options";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "0x";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "0x";
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.Location = new System.Drawing.Point(160, 160);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(288, 20);
            this.txtInput.TabIndex = 6;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(144, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 23);
            this.label4.TabIndex = 18;
            this.label4.Text = "0x";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(160, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(280, 32);
            this.label5.TabIndex = 19;
            this.label5.Text = "Input for Write Transactions (hex):  For Block transfer, please enter in little e" +
    "ndian mode";
            // 
            // AddrSpaceTransferForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1071, 590);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkBoxInc);
            this.Controls.Add(this.cmboMode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTransType);
            this.Controls.Add(this.lblBar);
            this.Controls.Add(this.btRead);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.lblNumBytes);
            this.Controls.Add(this.txtNumBytes);
            this.Controls.Add(this.txtOffset);
            this.Controls.Add(this.cmboTransType);
            this.Controls.Add(this.cmboBar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AddrSpaceTransferForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Read/Write Address Space Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
#endregion

        public bool GetInput()
        {            
            m_obj = new object[1];

            DialogResult result = DialogResult.Retry;

            while((result = ShowDialog()) == DialogResult.Retry);//������ʾ����

            return true;
        }

        private void btRead_Click(object sender, System.EventArgs e) //read�¼�����
        {
            m_direction = RW.READ;
            btClick();            
        }

        private void btWrite_Click(object sender, System.EventArgs e)//д�¼�
        {
            m_direction = RW.WRITE;
            btClick();                
        }

        private void btClick()
        {
            DialogResult = DialogResult.None;
            try
            {
                TranslateInput();
            }
            catch 
            {
                MessageBox.Show(m_excp.Message, "Input Entry Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DialogResult = DialogResult.Retry;//������ʾ
                return;
            }
 
            ReadWriteAddrSpace();    
        }

        private void ReadWriteAddrSpace()
        {
            DWORD dwStatus = 0;
            BOOL bIsBlock = (m_type == TRANSFER_TYPE.BLOCK);//�ж��Ƿ�������ģʽ
            BOOL bIsRead = (m_direction == RW.READ);//�ж��Ƕ�����д
            WDC_ADDR_RW_OPTIONS dwOptions = (m_bAutoInc ?
              //��дģʽ �Զ����� ���ΪĬ��ģʽ ��ô���Զ����������ַ������ �����no_autoincģʽ��ô��Ҫ�ڿ鴫�����Զ����Ӷ�/д��ַ��
              //���ڶ�/дһ��洢����I / O��ַ������飨����������أ�ʱ�����豸��ַ���䡣             
              WDC_ADDR_RW_OPTIONS.WDC_ADDR_RW_DEFAULT : 
                WDC_ADDR_RW_OPTIONS.WDC_ADDR_RW_NO_AUTOINC);
            /*WDC_ADDR_RW_NO_AUTOINC Do no automatically increment the read/write address 
           * in block transfers, i.e., hold the device address constant
           *while reading / writing a block of memory or I/ O
           *addresses(relevant only for block(string) transfers).*/
            DWORD dwFloorBytes = ((DWORD)(m_dwBytes / (DWORD)m_mode)) * 
                (DWORD)m_mode;//??????????

            switch(m_mode)
            {
            case WDC_ADDR_MODE.WDC_MODE_8:
                {
                    if(bIsRead)
                        dwStatus = bIsBlock ? 
                            wdc_lib_decl.WDC_ReadAddrBlock(m_device.Handle,    //B.3.24. WDC_ReadAddrBlock()  Reads a block of data from the device.
                                m_dwBar, m_dwOffset, dwFloorBytes, m_bData,  //m_dwOffset  The offset from the beginning of the specified address space(dwAddrSpace) to read from
                                m_mode, dwOptions) :                                           //��ָ����ַ�ռ䣨dwAddrSpace����ʼ��ƫ�������ж�ȡ
                            wdc_lib_decl.WDC_ReadAddr8(m_device.Handle, 
                                m_dwBar, m_dwOffset, ref m_bData[0]);
                    else
                        dwStatus = bIsBlock?
                            wdc_lib_decl.WDC_WriteAddrBlock(m_device.Handle, 
                                m_dwBar, m_dwOffset, dwFloorBytes, m_bData, 
                                m_mode, dwOptions) :
                            wdc_lib_decl.WDC_WriteAddr8(m_device.Handle,
                                m_dwBar, m_dwOffset, m_bData[0]);//����nonblock���� ֻ��һ��Ϊ˵��Ҫ��[0]

                    m_obj[0] = m_bData; 
                    break;
                }
            case WDC_ADDR_MODE.WDC_MODE_16:
                {
                    if(bIsRead)
                        dwStatus = bIsBlock ? 
                            wdc_lib_decl.WDC_ReadAddrBlock(m_device.Handle, 
                                m_dwBar, m_dwOffset, dwFloorBytes, m_wData, 
                                m_mode, dwOptions) :
                            wdc_lib_decl.WDC_ReadAddr16(m_device.Handle, 
                                m_dwBar, m_dwOffset, ref m_wData[0]);
                    else
                        dwStatus = bIsBlock?
                            wdc_lib_decl.WDC_WriteAddrBlock(m_device.Handle, 
                                m_dwBar, m_dwOffset, dwFloorBytes, m_wData, 
                                m_mode, dwOptions) :
                            wdc_lib_decl.WDC_WriteAddr16(m_device.Handle, 
                                m_dwBar, m_dwOffset, m_wData[0]);

                    m_obj[0] = m_wData; //��������������  object��
                    break;
                }
            case WDC_ADDR_MODE.WDC_MODE_32:
                {
                    if(bIsRead)
                        dwStatus = bIsBlock ? 
                            wdc_lib_decl.WDC_ReadAddrBlock(m_device.Handle,
                                m_dwBar, m_dwOffset, dwFloorBytes, m_u32Data,
                                m_mode, dwOptions) :
                            wdc_lib_decl.WDC_ReadAddr32(m_device.Handle, 
                                m_dwBar, m_dwOffset, ref m_u32Data[0]);
                    else
                        dwStatus = bIsBlock?
                            wdc_lib_decl.WDC_WriteAddrBlock(m_device.Handle, 
                                m_dwBar, m_dwOffset, dwFloorBytes, m_u32Data,
                                m_mode, dwOptions) :
                            wdc_lib_decl.WDC_WriteAddr32(m_device.Handle, 
                                m_dwBar, m_dwOffset, m_u32Data[0]);

                    m_obj[0] = m_u32Data; 
                    break;
                }
            case WDC_ADDR_MODE.WDC_MODE_64:
                {
                    if(bIsRead)
                        dwStatus = bIsBlock ? 
                            wdc_lib_decl.WDC_ReadAddrBlock(m_device.Handle, 
                                m_dwBar, m_dwOffset, dwFloorBytes, m_u64Data,
                                m_mode, dwOptions) :
                            wdc_lib_decl.WDC_ReadAddr64(m_device.Handle, 
                                m_dwBar, m_dwOffset, ref m_u64Data[0]);
                    else
                        dwStatus = bIsBlock?
                            wdc_lib_decl.WDC_WriteAddrBlock(m_device.Handle, 
                                m_dwBar, m_dwOffset, dwFloorBytes, m_u64Data,
                                m_mode, dwOptions) :
                            wdc_lib_decl.WDC_WriteAddr64(m_device.Handle, 
                                m_dwBar, m_dwOffset, m_u64Data[0]);

                    m_obj[0] = m_u64Data; 
                    break;
                }
            }

            TraceLog(bIsRead, (wdc_err)dwStatus);            
        }        

        private void TranslateInput()
        {
            string str = "";

            m_excp = new Exception("��ѡ��һ��BAR�ռ�");//�쳣��ʾ û��ѡ��  ��û��ѡ���ʱ����
            if((uint)cmboBar.SelectedIndex == 0xffffffff)//���û��ѡ��һ��bar ���׳�һ������ ͨ���ж��ǲ���-1���ж�
                throw m_excp;//�׳��쳣
            m_dwBar = (DWORD)cmboBar.SelectedIndex;//����ѡ�����Ŀ

            m_excp = new Exception("��ѡ����λ��");
            uint uiModeIndex = (uint)cmboMode.SelectedIndex;
            if(uiModeIndex == 0xffffffff)
                throw m_excp;//�׳��쳣
            m_mode = (uiModeIndex == 0)? WDC_ADDR_MODE.WDC_MODE_8: 
                (uiModeIndex == 1)? WDC_ADDR_MODE.WDC_MODE_16:
                (uiModeIndex == 2)? WDC_ADDR_MODE.WDC_MODE_32:
                WDC_ADDR_MODE.WDC_MODE_64;//ģʽѡ�� �����ж�

            m_excp = new Exception("Choose the Transfer Type");//�������� block nonblock����
            if((uint)cmboTransType.SelectedIndex == 0xffffffff)
                throw m_excp;
            m_type = (cmboTransType.SelectedIndex == 0)?
            TRANSFER_TYPE.BLOCK : TRANSFER_TYPE.NONBLOCK;//�ж���block ����nonblock���͵� 
            /*m_dwBytes�����������ݵĸ��� ����������8bit ������16����32 64bit ����nonblock���� ����Ϊһλ ����block���Ϳ����Ǵ���һ�ĸ��� ����m_dwBytes����
             * byte������ ��ͬ��ģʽ�����һ��ת��   �²� ����block����������д���� ����һ��һ����д 
             */

            if(txtNumBytes.Enabled == true)//��enabled֮�� ���Ϊture ������������� false������������
            {
                m_excp = new Exception("Please enter the number of bytes. " +
                    "Entered value should be a hex number." + 
                    Environment.NewLine + " (Maximum value: 0x" +
                    m_device.AddrDesc[m_dwBar].dwBytes.ToString("X") + ")");//���Ŀռ�
                m_dwBytes = Convert.ToUInt32(txtNumBytes.Text,16);//��16����ת��Ϊ32λ�޷�������
                if(m_dwBytes > m_device.AddrDesc[m_dwBar].dwBytes)//�ж��Ƿ񳬳���Χ    dwBytes��The address space's size (in bytes)
                    throw m_excp;
            }
            else 
                m_dwBytes = (DWORD)((m_mode == WDC_ADDR_MODE.WDC_MODE_8) ? 1 :
                    ((m_mode == WDC_ADDR_MODE.WDC_MODE_16) ? 2 :
                    ((m_mode == WDC_ADDR_MODE.WDC_MODE_32) ? 4 : 8)));

            if(chkBoxInc.Enabled == true)//���ſ�  ����жϿ��Ƿ�ѡ��
                m_bAutoInc = chkBoxInc.Checked;//�Զ����� 

            m_excp = new Exception("Please enter the offset. " +
                "Entered value should be a hex number");
            m_dwOffset = (DWORD)Convert.ToInt32(txtOffset.Text,16);//offset ������  ���û���������� �ͻᴥ���쳣
            //m_direction������ʾ��д�¼����Ƕ��¼�
            if(m_direction == RW.WRITE && txtInput.Text == "")
            {//�����д�¼�����û���������� �׳��쳣
                m_excp = new Exception("You must enter the data to write. " +
                    "data should be hex");
                throw m_excp;
            }

            m_excp = new Exception("The data you've entered is invalid. please "
                + "try again (hex)");
            
            switch(m_mode)//m_bData  m_wData  m_u32Data ���Ͳ�һ��
            {
            case WDC_ADDR_MODE.WDC_MODE_8:
                {    
                    m_bData = new byte[m_dwBytes];

                    if(m_direction == RW.WRITE)
                    {
                        str = txtInput.Text;//��������??
                        for(int i=0, j=0; i<str.Length && j<m_dwBytes; j++)
                        {
                            while (str[i] == ' ') ++i;//���������ո�
                            m_bData[j] = Convert.ToByte(str.Substring(i,2),16);//���ݳ�����mode����  8bit�����鳤��Ϊ1 ԪΪbyte 16bit�����鳤��Ϊ1 ԪΪ32λ
                            i+=2;    
                        }
                    }                        
                    break;
                }
            case WDC_ADDR_MODE.WDC_MODE_16:
                {
                    m_wData = new WORD[m_dwBytes/2];

                    if(m_direction == RW.WRITE)
                    {
                        str = txtInput.Text;
                        for(int i=0, j=0; i<str.Length && j<m_dwBytes/2; j++)
                        {
                            while (str[i] == ' ') ++i;
                            m_wData[j] = Convert.ToUInt16(str.Substring(i,4),16);//substring����һ����startIndex��ʼ������Ϊlength�����ַ�����
                                i +=4;
                        }        
                    }
                    break;
                }
            case WDC_ADDR_MODE.WDC_MODE_32://���з�֧ѭ��һ�� ��װ�ɲ�ͬλ����ֵ
                {
                    m_u32Data = new UINT32[m_dwBytes/4];

                    if(m_direction == RW.WRITE)
                    {
                        str = txtInput.Text;
                        for(int i=0, j=0; i<str.Length && j<m_dwBytes/4; j++)
                        {
                            while (str[i] == ' ') ++i;
                            m_u32Data[j] = Convert.ToUInt32(str.Substring(i,8),
                                16);
                            i+=8;
                        }
                    }                        
                    break;
                }
            case WDC_ADDR_MODE.WDC_MODE_64:
                {
                    m_u64Data = new UINT64[m_dwBytes/8];

                    if(m_direction == RW.WRITE)
                    {
                        str = txtInput.Text;
                        for(int i=0, j=0; i<str.Length && j<m_dwBytes/8; j++)
                        {
                            while (str[i] == ' ') ++i;
                            m_u64Data[j] = Convert.ToUInt64(str.Substring(i,2),
                                16);
                            i+=16;
                        }
                    }                        
                    break;
                }
            }                    
        }

        private void cmboTransType_SelectedIndexChanged(object sender, //����ѡ��
            System.EventArgs e)
        {
            if((string)cmboTransType.SelectedItem == "non-block")
            {
                txtNumBytes.Enabled = false;
                chkBoxInc.Enabled = false;
            }
            else if((string)cmboTransType.SelectedItem == "block")
            {
                txtNumBytes.Enabled = true;
                chkBoxInc.Enabled = true;
            }
        }        

        private void btLog_Click(object sender, System.EventArgs e)
        {
            txtData.Clear();
        }    

        private void TraceLog(BOOL bIsRead, wdc_err status)
        {
            string sData = "";
            string sInfo = "";
            if (status == wdc_err.WD_STATUS_SUCCESS)//����ɹ��Ļ� 
            {
                sData = (bIsRead? "R: " : "W: ") + 
                    diag_lib.DisplayHexBuffer(m_obj, m_dwBytes, m_mode);//д������װ�����ַ���
                sInfo = (bIsRead ? " from " : " to ") +    "offset " + 
                    m_dwOffset.ToString("X") + " on BAR " + m_dwBar.ToString() //д���Ƕ�����д ��ʲôλ�� �Ǹ��豸
                    + "(" + m_device.ToString(false) + ")";

                Log.TraceLog("AddrSpaceTransferForm: " + sData + sInfo);//����һ����ӡ
            }
            else 
            {
                sData = "failed to " + (bIsRead? "read from" : "write to") + 
                    " offset " + m_dwOffset.ToString("X") + " on BAR " + 
                    m_dwBar.ToString() + "status 0x" + status.ToString("X") + 
                    ": " + utils.Stat2Str((DWORD)status);
                                sInfo = "(" + m_device.ToString(false) + ")";

                Log.ErrLog("AddrSpaceTransferForm: " + sData + sInfo);
            }

            txtData.Text += sData + Environment.NewLine;            //����һ����ӡ�ı� ����д��
        }

        private void lblOffset_Click(object sender, EventArgs e)
        {

        }

        private void txtOffset_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNumBytes_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmboBar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmboMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

