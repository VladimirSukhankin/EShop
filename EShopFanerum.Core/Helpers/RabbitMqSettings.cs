﻿using System.Xml;

namespace EShopFanerum.Core.Helpers;

public class RabbitMqSettings
{
     /// <summary>
        /// Включен/выключен клиент RabbitMQ.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Адреса серверов, разделенные ";".
        /// </summary>
        public string HostNames { get; set; }

        /// <summary>
        /// Номер порта.
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Виртуальный хост.
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Производить ли запись сообщений на жесткий диск.
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// Удалять ли обменник после обработки последнего сообщения.
        /// </summary>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// Количество попыток.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Интервал попыток.
        /// </summary>
        public TimeSpan RetryIntervalValue { get; set; }

        /// <summary>
        /// <see cref="RetryIntervalValue"/>.
        /// </summary>
        public string RetryInterval
        {
            get
            {
                return XmlConvert.ToString(RetryIntervalValue);
            }
            set
            {
                RetryIntervalValue = XmlConvert.ToTimeSpan(value);
            }
        }

        /// <summary>
        /// Настройки потребителя сообщений по умолчанию.
        /// </summary>
        public ConsumerSettings DefaultConsumer { get; set; }

        /// <summary>
        /// Настройки конкретных потребителей сообщений, которые настраиваются отдельно от общих.
        /// </summary>
        public ConsumerSettings[] ConsumerGroups { get; set; }

        /// <summary>
        /// Включить или отключить пакетную отправку.
        /// </summary>
        public bool BatchPublishEnabled { get; set; }

        /// <summary>
        /// Время ожидания дополнительных сообщений перед пакетной отправкой в миллисекундах (от 0 до 1000).
        /// </summary>
        public int? BatchPublishTimeoutMilliseconds { get; set; }
}