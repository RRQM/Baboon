using System.Diagnostics;

namespace Baboon.Winform;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        await Task.Run(async () =>
        {
            //模拟耗时操作
            for (int i = 0; i < 1000; i++)
            {
                Debug.WriteLine(i);
            }

            //切换到主线程
            await MainThreadTaskFactory.SwitchToMainThreadAsync();

            //更新UI
            this.Text = "Hello";
        });
        await Task.Run(async () =>
        {
            //this.Text = "1132";
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine("这里是线程池线程执行" + Thread.CurrentThread.ManagedThreadId);
            }
            using (CancellationTokenSource tokenSource = new CancellationTokenSource(2000))
            {
                //await Task.Delay(3000);

                try
                {
                    await MainThreadTaskFactory.SwitchToMainThreadAsync(tokenSource.Token);
                    this.Text = "1111";
                    Debug.WriteLine("这里是主线程执行" + Thread.CurrentThread.ManagedThreadId);

                    await MainThreadTaskFactory.ReleaseMainThreadAsync();
                    Debug.WriteLine("主线程之后执行" + Thread.CurrentThread.ManagedThreadId);

                    //下面的设置应该不会生效，因为是在其他线程操作的
                    this.Text = "2222";
                }
                catch (Exception ex)
                {


                }

            }

        });
    }
}
