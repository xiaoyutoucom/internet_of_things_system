using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Robin.Domain.Entities;
using Robin.NHibernate.EntityMappings;

namespace SmartKylinData.IOTModel
{
    public class VideoRecordMap : EntityMap<VideoRecord>
    {
        public VideoRecordMap() : base("smart_kylin_video")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.SPDBM);
            Map(x => x.SPDMC);
            Map(x => x.SPPP);
            Map(x => x.FZID);
            Map(x => x.XSQS);
            Map(x => x.IPLX);
            Map(x => x.IP);
            Map(x => x.DKH);
            Map(x => x.YHM);
            Map(x => x.MM);
            Map(x => x.X);
            Map(x => x.Y);
            Map(x => x.SPJXLXBM);
            Map(x => x.SPJXLX);
            Map(x => x.JXCS);
            Map(x => x.SPJXLXBM2);
            Map(x => x.SPJXLX2);
            Map(x => x.JXCS2);
            Map(x => x.BZ);
            Map(x => x.EXTENDCODE);
            Map(x => x.EXTENDCODE2);
            Map(x => x.EXTENDCODE3);
            Map(x => x.EXTENDCODE4);
            Map(x => x.EXTENDCODE5);
        }
    }
    /// <summary>
    /// 视频点
    /// </summary>

    public class VideoRecord : Entity<int>
    {
        /// <summary>
        /// 视频点编码
        /// </summary>
        public virtual string SPDBM { get; set; }
        /// <summary>
        /// 视频点名称
        /// </summary>
        public virtual string SPDMC { get; set; }
        /// <summary>
        /// 设备品牌
        /// </summary>
        public virtual string SPPP { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public virtual int FZID { get; set; }
        /// <summary>
        /// 显示器数
        /// </summary>
        public virtual long XSQS { get; set; }
        /// <summary>
        /// IP类型
        /// </summary>
        public virtual string IPLX { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public virtual string IP { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public virtual string DKH { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string YHM { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string MM { get; set; }
        /// <summary>
        /// X坐标
        /// </summary>
        public virtual decimal X { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public virtual decimal Y { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public virtual decimal XH { get; set; }
        /// <summary>
        /// 视频解析编码
        /// </summary>
        public virtual string SPJXLXBM { get; set; }
        /// <summary>
        /// 视频解析类型
        /// </summary>
        public virtual string SPJXLX { get; set; }
        /// <summary>
        /// 解析参数
        /// </summary>
        public virtual string JXCS { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string BZ { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public virtual string EXTENDCODE { get; set; }
        /// <summary>
        /// 视频解析编码2
        /// </summary>
        public virtual string SPJXLXBM2 { get; set; }
        /// <summary>
        /// 视频解析类型2
        /// </summary>
        public virtual string SPJXLX2 { get; set; }
        /// <summary>
        /// 解析参数2
        /// </summary>
        public virtual string JXCS2 { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
        public virtual string EXTENDCODE2 { get; set; }
        /// <summary>
        /// 备用字段3
        /// </summary>
        public virtual string EXTENDCODE3 { get; set; }
        /// <summary>
        /// 备用字段4
        /// </summary>
        public virtual string EXTENDCODE4 { get; set; }
        /// <summary>
        /// 备用字段5
        /// </summary>
        public virtual string EXTENDCODE5 { get; set; }


    }
}
