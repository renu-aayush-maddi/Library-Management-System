using LibraryManagementSystem.Models;
using LibraryManagementSystem.Interfaces;
using System.Text.RegularExpressions;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Services;

public class MemberService
{
    private readonly IMemberRepository memberRepository;
    private readonly AppDbContext context;

    public MemberService(IMemberRepository memberRepository ,  AppDbContext context)
    {
        this.memberRepository = memberRepository;
        this.context = context;
    }

    public List<Member> GetAllMembers()
    {
        return memberRepository.GetAllMembers();
    }

    public void AddMember(Member member)
    {
        if (string.IsNullOrWhiteSpace(member.Fullname))
        {
            throw new Exception("Name is required");
        }

        if (!Regex.IsMatch(member.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new Exception("Invalid email format");
        }

        if(!Regex.IsMatch(member.Phone,@"^[0-9]{10}$"))
        {
            throw new Exception("Phone number must contain 10 digits");
        }

        Membershiptype? membershipType = context.Membershiptypes
                                        .FirstOrDefault(m => m.Membershiptypeid == member.Membershiptypeid);

        if (membershipType == null)
        {
            throw new Exception("Invalid membership type");
        }

        bool emailExists = context.Members.Any(m => m.Email == member.Email);

        if (emailExists)
        {
            throw new Exception("Email already exists");
        }

        bool phoneExists = context.Members.Any(m => m.Phone == member.Phone);

        if (phoneExists)
        {
            throw new Exception("Phone number already exists");
        }
        
        memberRepository.AddMember(member);
    }

    public Member? GetMemberById(int memberId)
    {
        return memberRepository.GetMemberById(memberId);
    }

    public Member? SearchMember(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            throw new Exception("Keyword is required");
        }

        return memberRepository.SearchMember(keyword);
    }

    public void DeactivateMember(int memberId)
    {
        Member? member = memberRepository.GetMemberById(memberId);

        if (member == null)
        {
            throw new Exception("Member not found");
        }

        memberRepository.DeactivateMember(member);
    }

    public void UpdateMembership(int memberId, int membershipTypeId)
    {
        Member? member = memberRepository.GetMemberById(memberId);

        if (member == null)
        {
            throw new Exception("Member not found");
        }

        Membershiptype? membershipType = context.Membershiptypes
            .FirstOrDefault(m => m.Membershiptypeid == membershipTypeId);

        if (membershipType == null)
        {
            throw new Exception("Invalid membership type");
        }

        member.Membershiptypeid = membershipTypeId;

        memberRepository.UpdateMember(member);
    }
}