# NLog.QiNiuLogTarget

NLog集成七牛云智能日志上传

#### 1 Nuget
```
PM> Install-Package YrinLeung.NLog.QiNiuLogTarget
```
支持 netstandard2.0

#### 2 使用方法

> NLog的Target配置

```xml
 <target xsi:type="QiNiuLogService"
           name="qiniulog"
           authorization="test-authorization"
           queueName="queue_service"//
           layout="serviceName=testservice1	message=${message}	logLevel=${level}	stackTrace=${stacktrace}	date=${date}" />

```
> 参数说明


参数 | 必填 | 说明
---|---|---
authorization | 是 | api签名信息，详情请查看[API签名接口](https://developer.qiniu.com/insight/api/4814/the-api-signature)。或用[官方工具](http://pandora-toolkits.qiniu.com/?ref=developer.qiniu.com)生成
queueName | 是 | 队列名称（实时仓库名称）

#### 3 注意
layout中的内容为七牛云数据推送接口中的请求内容，详情请查看[官方API](https://developer.qiniu.com/insight/api/4749/data-push-api)

#### 4 Demo
查看 [Demo](https://github.com/yrinleung/NLog.QiNiuLogTarget/tree/master/test)
