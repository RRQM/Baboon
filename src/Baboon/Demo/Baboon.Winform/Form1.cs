using System.Diagnostics;

namespace Baboon.Winform;

public partial class Form1 : Form
{
    public Form1()
    {
        var ssss = Thread.CurrentThread;
        var ss = SynchronizationContext.Current;
        InitializeComponent();

        var s22s = SynchronizationContext.Current;
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        await Task.Run(async () =>
        {
            //this.Text = "1132";
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine("�������̳߳��߳�ִ��" + Thread.CurrentThread.ManagedThreadId);
            }
            using (CancellationTokenSource tokenSource = new CancellationTokenSource(2000))
            {
                //await Task.Delay(3000);

                try
                {
                    await MainThreadTaskFactory.SwitchToMainThreadAsync(tokenSource.Token);
                    this.Text = "1111";
                    Debug.WriteLine("���������߳�ִ��" + Thread.CurrentThread.ManagedThreadId);

                    await MainThreadTaskFactory.ReleaseMainThreadAsync();
                    Debug.WriteLine("���߳�֮��ִ��" + Thread.CurrentThread.ManagedThreadId);

                    //���������Ӧ�ò�����Ч����Ϊ���������̲߳�����
                    this.Text = "2222";
                }
                catch (Exception ex)
                {


                }

            }

        });
    }
}
