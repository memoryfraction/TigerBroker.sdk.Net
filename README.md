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

-  自行编译法

下载src, 安装Visual Studio，编译源码并获取TigerBrokerLibrary.dll, 然后可以使用。



- Nuget包安装法

待完成



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



### 预期结果

![Image](E:\GitHub\TigerBrokerLib\README.assets\Image.png)





## 支持

### 支付宝

<img src="E:\GitHub\TigerBrokerLib\README.assets\支付宝二维码.jpg" alt="支付宝二维码" style="zoom:50%;" />

### Venmo

<img src="E:\GitHub\TigerBrokerLib\README.assets\Venmo.jpg" alt="Venmo" style="zoom:50%;" />

### 老虎推荐码

JJEH3



## 其他

如果使用人数较多，比如：收藏过千，会考虑继续开发行情数据的获取和自动化交易等特性。



