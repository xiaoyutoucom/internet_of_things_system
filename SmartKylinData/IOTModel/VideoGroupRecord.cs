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
    public class VideoGroupRecordMap : EntityMap<VideoGroupRecord>
    {
        public VideoGroupRecordMap() : base("smart_kylin_videogroup")
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.FID);
            Map(x => x.GCODE);
            Map(x => x.FZMC);
            Map(x => x.BZ);
            Map(x => x.XH);
        }
    }
    /// <summary>
    /// 视频分组
    /// </summary>
    public class VideoGroupRecord : Entity<int>
    {
        /// <summary>
        /// 父节点编号
        /// </summary>
        public virtual int FID { get; set; }
        /// <summary>
        /// 分组编号
        /// </summary>
        public virtual string GCODE { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public virtual string FZMC { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string BZ { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public virtual long XH { get; set; }
        

    }
}
