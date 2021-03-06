﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 提供 Tooltip 功能的组件
    /// </summary>
    public abstract class TooltipComponentBase : IdComponentBase, ITooltipHost, IDisposable
    {
        /// <summary>
        /// 获得/设置 ITooltip 实例
        /// </summary>
        public ITooltip? Tooltip { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            // 初始化 Tooltip 组件
            if (Tooltip != null)
            {
                if (AdditionalAttributes == null) AdditionalAttributes = new Dictionary<string, object>();
                AdditionalAttributes["data-placement"] = Tooltip.Placement.ToDescriptionString();

                if (!AdditionalAttributes.TryGetValue("data-trigger", out var _) && !string.IsNullOrEmpty(Tooltip.Trigger))
                {
                    AdditionalAttributes["data-trigger"] = Tooltip.Trigger;
                }
            }
        }

        /// <summary>
        /// OnAfterRenderAsync
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender && Tooltip != null)
            {
                JSRuntime.Tooltip(Id, "", Tooltip.PopoverType, RetrieveTitle(), RetrieveContent(), RetrieveIsHtml());
            }
        }

        /// <summary>
        /// 获得 弹窗标题方法
        /// </summary>
        /// <returns></returns>
        protected virtual string RetrieveTitle()
        {
            return Tooltip != null ? Tooltip.Title : "";
        }

        /// <summary>
        /// 获得 弹窗内容方法
        /// </summary>
        /// <returns></returns>
        protected virtual string RetrieveContent()
        {
            return Tooltip != null
                ? (Tooltip.PopoverType == PopoverType.Popover ? Tooltip.Content : "")
                : "";
        }

        /// <summary>
        /// 获得 弹窗内容是否为 Html 方法
        /// </summary>
        /// <returns></returns>
        protected virtual bool RetrieveIsHtml()
        {
            return Tooltip?.IsHtml ?? false;
        }

        /// <summary>
        /// Dispose 方法
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && Tooltip != null)
            {
                JSRuntime.Tooltip(Id, "dispose", popoverType: Tooltip.PopoverType);
            }
        }
    }
}
