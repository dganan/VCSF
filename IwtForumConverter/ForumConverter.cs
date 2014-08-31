using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

using WsIwt = Moma.IwtForumConverter;

namespace Moma.IwtForumConverter
{
    public class ForumConverter  :  VCS.Converter
    {
        WsIwt.CollaborativeSessionExportServiceClient webServiceClient;

        public ForumConverter()
        {

            webServiceClient = new WsIwt.CollaborativeSessionExportServiceClient();

            Mapper.CreateMap<WsIwt.Site, VCS.Site>();
            Mapper.CreateMap<WsIwt.CollaborativeSessionDescriptor, VCS.CollaborativeSessionDescriptor>();
            Mapper.CreateMap<WsIwt.CS2Object, VCS.CS2Object>();
            Mapper.CreateMap<WsIwt.CollaborativeSession, VCS.CollaborativeSession>();
            Mapper.CreateMap<WsIwt.Post, VCS.Post>();
            Mapper.CreateMap<WsIwt.Category, VCS.Category>();
            Mapper.CreateMap<WsIwt.Role, VCS.Role>();
            Mapper.CreateMap<WsIwt.UserAccount,  VCS.UserAccount>();
            Mapper.CreateMap<WsIwt.Gender, VCS.Gender>();
         
            Mapper.CreateMap<VCS.CS2Object, WsIwt.CS2Object>()
             .ForMember(dest => dest.ExtensionData,  opt => opt.Ignore());
            Mapper.CreateMap<VCS.CollaborativeSessionDescriptor, WsIwt.CollaborativeSessionDescriptor>()
                 .ForMember(dest => dest.ExtensionData,  opt => opt.Ignore());
            Mapper.CreateMap<VCS.CollaborativeSession, WsIwt.CollaborativeSession>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());
            Mapper.CreateMap<VCS.Post, WsIwt.Post>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());
            Mapper.CreateMap<VCS.Category, WsIwt.Category>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());
            Mapper.CreateMap<VCS.Role, WsIwt.Role>();
                //.ForMember(dest => dest.ExtensionData, opt => opt.Ignore());
            Mapper.CreateMap<VCS.UserAccount, WsIwt.UserAccount>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore()  )  ;
            Mapper.CreateMap<VCS.Site, WsIwt.Site>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());
            Mapper.AssertConfigurationIsValid();
        }

        public override List<VCS.CollaborativeSessionDescriptor> AvailableCollaborativeSessions
        {
            get
            {
                return
                    Mapper.Map<List<WsIwt.CollaborativeSessionDescriptor>, List<VCS.CollaborativeSessionDescriptor>>(
                        webServiceClient.AvailableCollaborativeSessions()
                    );
            }
        }

        protected override VCS.CollaborativeSession ReadCollaborativeSessionData(string csId)
        {
			VCS.CollaborativeSessionDescriptor cs = new VCS.CollaborativeSessionDescriptor() { Id = csId };

            WsIwt.CollaborativeSessionDescriptor collSession = Mapper.Map<VCS.CollaborativeSessionDescriptor, WsIwt.CollaborativeSessionDescriptor>(cs);

            return
                Mapper.Map<WsIwt.CollaborativeSession, VCS.CollaborativeSession>(
                    webServiceClient.ReadCollaborativeSessionData(
                        collSession
                    )
                    );
        }

        protected override List<VCS.UserAccount> ReadUserAccounts(string csId)
        {
			VCS.CollaborativeSessionDescriptor cs = new VCS.CollaborativeSessionDescriptor() { Id = csId };
			
			return
                Mapper.Map < List<WsIwt.UserAccount>, List<VCS.UserAccount>>(
                    webServiceClient.ReadUserAccounts(
                        Mapper.Map<VCS.CollaborativeSessionDescriptor, WsIwt.CollaborativeSessionDescriptor>(cs)
                    )
                );
        }

		protected override List<VCS.Post> ReadPosts(string csId)
        {
			VCS.CollaborativeSessionDescriptor cs = new VCS.CollaborativeSessionDescriptor() { Id = csId };

            DataTransformationMgmt dataTransf = new DataTransformationMgmt();

            List<WsIwt.Message> messages = webServiceClient.ReadPosts(Mapper.Map<VCS.CollaborativeSessionDescriptor, WsIwt.CollaborativeSessionDescriptor>(cs));

            dataTransf.CreatePost(messages);

            List<WsIwt.Post> posts = messages.Cast<WsIwt.Post>().ToList();

            return Mapper.Map<List<WsIwt.Post>, List<VCS.Post>>(posts);

        }

    }
}
