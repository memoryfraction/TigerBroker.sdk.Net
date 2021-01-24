# TigerBrokerLib

简介: 使用该类库可以与老虎证券通信，自动获得用户的账户市值信息。 





## 依赖项

- [.NET Standard 2.1](https://dotnet.microsoft.com/download/dotnet-framework)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) 12.0.3版本或以上



## 特点

跨平台可用， 比如： windows, Linux, Mac



## 准备工作

- 获取开发者信息

  进入[老虎量化](https://quant.itiger.com/openapi/py-docs/zh-cn/docs/intro/quickstart.html)开发者页面，按照页面提示，注册成为开发者。并确保获取如下信息

  RSA公钥：比如存为PublicKey.txt

  RSA私钥：比如存为PrivateKey.txt

  account: 用来区分用户的账户类型，用户交易时选择对应的账户类型即可

  tigerId： 用来唯一标识一个开发者，在请求API接口时需要该参数

  建议使用client-secrets.json存储私密信息， 格式如下
  
  ```json
  {
     "account": "XXXXX",
     "tiger_id": "XXX"
  }
  ```
  
  
  
  

## 使用方法

### TigerBrokerLib安装方法

-  自行编译

下载src, 安装Visual Studio，编译源码并获取TigerBrokerLibrary.dll, 然后可以使用。



- [Nuget包](https://www.nuget.org/packages/TigerBrokerLibrary/)安装

```c#
Install-Package TigerBrokerLibrary -Version 1.0.0
```



### client-secrets.json使用方法

假定client-secrets.json文件相对路径: "Data/client-secrets.json"

```c#
// 定义
var config = new ConfigurationBuilder()
                .AddJsonFile("Data/client-secrets.json")
                .Build();
// 使用
var tiger_id = config["tiger_id"];
```



### 获取老虎证券账户信息的方法
```c#
            //1 Generate Dictionary
            //  Convert time zone to the one in Chinese
            var chinaZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            var dtChina = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.Date, chinaZone);
            var timeStamp = dtChina.ToString("yyyy-MM-dd HH:mm:ss");

            var url = "https://openapi.itiger.com/gateway";
            var account = config["account"];
            var charset = "UTF-8";
            var method = "assets";
            var sign_type = "RSA";
            var tiger_id = config["tiger_id"];
            var version = "1.0";
            var paramsDic = _tigerBrokerService.GetTigerBrokerRequestDictionary(
                timeStampString: timeStamp, 
                account: account, 
                charset: charset, 
                method: method,
                sign_type : sign_type,
                tiger_id : tiger_id, 
                version : version);
    
            //2 Generate Sign
            //2.1 generate content to be signed
            var contentTobeSigned = _tigerBrokerService.GetContentTobeSigned(paramsDic);
    
            // 2.2 Sign
            var privateKey = File.ReadAllText(@"Data\TigerBrokerPrivateKey.txt");
            var sign = _tigerBrokerService.Sign(contentTobeSigned, privateKey);


            //3 Generate Request
            var tigerBrokerRequest = _tigerBrokerService.GetTigerAssetsRequest(
                account: account,
                tiger_id: tiger_id,
                timeStamp: timeStamp,
                sign: sign,
                method: method,
                charset: charset,
                sign_type:sign_type,
                version:version
                ); 
    
            //4 Send Request and get Response
            var tigerResponse = await _tigerBrokerService.GetTigerBrokerAssetsModelAsync(url,tigerBrokerRequest);
            var tigerAssets = _tigerBrokerService.ConverToTigerAssetsFromTigerAssetsResponse(tigerResponse);
```



### 客户端（单元测试）项目文件结构图

![Image](https://github.com/memoryfraction/TigerBrokerLib/blob/main/images/UnitTestFilesOrganization.png?raw=true)



### 预期结果

![Image](https://github.com/memoryfraction/TigerBrokerLib/blob/main/images/ExpectResult.png?raw=true)



## 声明

由于知识有限，精力有限，不对开源版本提供任何使用质量保障和服务。如有问题， 欢迎在[Issue](https://github.com/memoryfraction/TigerBrokerLib/issues)区提出。



## 捐赠与支持

- [支付宝](https://github.com/memoryfraction/TigerBrokerLib/blob/main/images/%E6%94%AF%E4%BB%98%E5%AE%9D%E4%BA%8C%E7%BB%B4%E7%A0%81.jpg?raw=true)
- [Venmo](https://github.com/memoryfraction/TigerBrokerLib/blob/main/images/Venmo.jpg)
- 老虎证券推荐码: JJEH3

