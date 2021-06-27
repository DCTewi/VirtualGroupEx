# Virtual Group Ex



Yet Another v Group Forum | 又一个v字幕组综合平台。

使用 Server-Side Blazor on ASP.NET Core with .NET 5.0.7，数据库使用SQLite 3。

## 一点有的没的

之前做的那个TewiClipool其实是数据库课的课程设计作业，虽然能用但是技术栈选的不太合适，而且写得太烂了。所以重做了这个，现在效果还可以吧。

开发过程其实是在另一个Repo，但是那个Commit太杂乱了，所以新开了一个。

## 编译

编译前需安装 [.NET 5.0 SDK](https://docs.microsoft.com/zh-cn/dotnet/core/install/linux)

#### 克隆代码

```shell
git clone https://github.com/DCTewi/VirtualGroupEx.git
```
#### 编译项目

```shell
#进入项目文件夹
cd VirtualGroupEx/

# 编译程序到 "published/"
dotnet publish -c Release -o published/ 

# 进入编译完成文件夹
cd published/
```

## 运行

```shell
# 启动 VirtualGroupEx
dotnet VirtualGroupEx.Server.dll
```

配置文件在 ` appsettings.json `

另外，反代需要[配置Web Socket](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/host-and-deploy/server?view=aspnetcore-5.0) 。
 

## 截图

截图可能不是最新的，可自行在软件内体验

[![RpH7VS.md.png](https://z3.ax1x.com/2021/06/18/RpH7VS.md.png)](https://imgtu.com/i/RpH7VS)
[![RpHob8.md.png](https://z3.ax1x.com/2021/06/18/RpHob8.md.png)](https://imgtu.com/i/RpHob8)
[![RpHIDf.md.png](https://z3.ax1x.com/2021/06/18/RpHIDf.md.png)](https://imgtu.com/i/RpHIDf)
