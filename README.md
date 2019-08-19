# NLog.QiNiuLogTarget

NLog集成七牛云智能日志上传

#### 1 Nuget
```
PM> Install-Package YrinLeung.NLog.QiNiuLogTarget
```
支持 netstandard2.0

#### 2 使用方法

> NLog的配置

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true">

  <extensions>
    <add assembly="NLog.QiNiuLogTarget"/>
  </extensions>
  
  <target>
    <target xsi:type="QiNiuLogService"
            name="qiniulog"
            authorization="test-authorization"
            queueName="queue_service">
      <parameter name="serviceName" layout="testservice1" />
      <parameter name="message" layout="${message}" />
      <parameter name="logLevel" layout="${level}" />
      <parameter name="date" layout="${date:format=yyyy-MM-ddTHH\:mm\:ss.fffZ}" />
    </target>
  </targets>
  <rules>
    <logger name="*" minlevel="Error" writeTo="qiniulog" />
  </rules>
</nlog>

```
> 参数说明


参数 | 必填 | 说明
---|---|---
authorization | 是 | api签名信息，详情请查看[API签名接口](https://developer.qiniu.com/insight/api/4814/the-api-signature)。或用[官方工具](http://pandora-toolkits.qiniu.com/?ref=developer.qiniu.com)生成
queueName | 是 | 队列名称（实时仓库名称）
parameter | 是 | 日志内容的字段，name=>字段名称，layout=>字段内容

#### 3 注意
- 七牛云的data类型需要转成“yyyy-MM-ddTHH\:mm\:ss.fffZ”


#### 4 Demo
查看 [Demo](https://github.com/yrinleung/NLog.QiNiuLogTarget/tree/master/test)
