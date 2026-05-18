using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext context;

    public MemberRepository(AppDbContext context)
    {
        this.context = context;
    }

    public List<Member> GetAllMembers()
    {
        return context.Members.ToList();
    }

    public void AddMember(Member member)
    {
        context.Members.Add(member);

        context.SaveChanges();
    }

    public Member? GetMemberById(int memberId)
    {
        return context.Members
            .Include(m => m.Membershiptype)
            .FirstOrDefault(m => m.Memberid == memberId);
    }

    public void UpdateMember(Member member)
    {
        context.Members.Update(member);

        context.SaveChanges();
    }

    public Member? SearchMember(string keyword)
    {
        return context.Members
            .Include(m => m.Membershiptype)
            .FirstOrDefault(m =>
                m.Fullname!.ToLower().Contains(keyword.ToLower()) ||
                m.Email!.ToLower().Contains(keyword.ToLower()) ||
                m.Phone!.Contains(keyword));
    }

    public void DeactivateMember(Member member)
    {
        member.Isactive = false;

        context.Members.Update(member);

        context.SaveChanges();
    }
}